using HG.GeneralSerializer;
using RoR2;
using RoR2EditorKit.Core.Inspectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace RoR2EditorKit.RoR2Related.Inspectors
{
    [CustomEditor(typeof(EntityStateConfiguration))]
    public sealed class EntityStateConfigurationInspector : ScriptableObjectInspector<EntityStateConfiguration>
    {
        #region Static methods and fields
        //For cases where the fieldInfo's type is not a unity object, we want to make specific visual elements and later serialize them as strings.
        private delegate VisualElement VisualElementCreationHandler(FieldInfo fieldInfo, object value);
        //Serialized fields can either be objects or strings, for cases where theyre not objects, we dont want to display a string field, instead we want to display a custom element.
        private static readonly Dictionary<Type, VisualElementCreationHandler> drawers = new Dictionary<Type, VisualElementCreationHandler>
        {
            [typeof(bool)] = (fld, vl) => CreateField<bool>(fld, vl, typeof(Toggle)),
            [typeof(long)] = (fld, vl) => CreateField<long>(fld, vl, typeof(LongField)),
            [typeof(int)] = (fld, vl) => CreateField<int>(fld, vl, typeof(IntegerField)),
            [typeof(float)] = (fld, vl) => CreateField<float>(fld, vl, typeof(FloatField)),
            [typeof(double)] = (fld, vl) => CreateField<double>(fld, vl, typeof(DoubleField)),
            //[typeof(string)] = (fld, vl) => CreateField<string>(fld, vl, typeof(TextField)),
            [typeof(Vector2)] = (fld, vl) => CreateField<Vector2>(fld, vl, typeof(Vector2Field)),
            [typeof(Vector3)] = (fld, vl) => CreateField<Vector3>(fld, vl, typeof(Vector3Field)),
            [typeof(Color)] = (fld, vl) => CreateField<Color>(fld, vl, typeof(ColorField)),
            [typeof(Color32)] = (fld, vl) => CreateField<Color32>(fld, vl, typeof(ColorField)),
            [typeof(AnimationCurve)] = (fld, vl) => CreateField<AnimationCurve>(fld, vl, typeof(CurveField)),
        };
        //This creates a visualElement from the given information, its basically ready for attatching to a container, except that it needs to be manually bound 
        private static VisualElement CreateField<T>(FieldInfo info, object value, Type elementType)
        {
            var element = (BaseField<T>)Activator.CreateInstance(elementType);
            element.name = $"{info.Name}_Property";
            element.label = ObjectNames.NicifyVariableName(info.Name);
            element.value = (T)value;
            return element;
        }
        //Sometimes an object needs a special creator, this does that
        private static readonly Dictionary<Type, Func<object>> specialDefaultValueCreators = new Dictionary<Type, Func<object>>
        {
            [typeof(AnimationCurve)] = () => new AnimationCurve(),
        };
        #endregion
        Type EntityStateType
        {
            get
            {
                return _entityStateType;
            }
            set
            {
                if (_entityStateType != value)
                {
                    _entityStateType = value;
                    OnEntityStateTypeSet();
                }
            }
        }
        Type _entityStateType = null;

        #region fields
        SerializedProperty collectionProp;
        SerializedProperty serializedFieldsProp;

        VisualElement inspectorDataContainer;
        VisualElement instanceFieldsContainer;
        VisualElement staticFieldsContainer;
        VisualElement unrecognizedFieldsContainer;

        List<FieldInfo> staticSerializableFields = new List<FieldInfo>();
        List<FieldInfo> instanceSerializableFields = new List<FieldInfo>();
        List<KeyValuePair<SerializedProperty, int>> unrecognizedFields = new List<KeyValuePair<SerializedProperty, int>>();
        #endregion
        protected override void OnEnable()
        {
            base.OnEnable();
            collectionProp = serializedObject.FindProperty(nameof(EntityStateConfiguration.serializedFieldsCollection));
            serializedFieldsProp = collectionProp.FindPropertyRelative(nameof(SerializedFieldCollection.serializedFields));

            OnVisualTreeCopy += () =>
            {
                var container = DrawInspectorElement.Q<VisualElement>("Container");
                inspectorDataContainer = container.Q<VisualElement>("InspectorDataContainer");
                instanceFieldsContainer = inspectorDataContainer.Q<VisualElement>("InstanceFieldsContainer");
                staticFieldsContainer = inspectorDataContainer.Q<VisualElement>("StaticFieldsContainer");
                unrecognizedFieldsContainer = inspectorDataContainer.Q<VisualElement>("UnrecognizedFieldsContainer");
            };
        }
        protected override void DrawInspectorGUI()
        {
            AddSimpleContextMenu(unrecognizedFieldsContainer, new ContextMenuData(
                $"Clear Unrecognized Fields",
                (act) =>
                {
                    foreach (var fieldRow in unrecognizedFields.OrderByDescending(el => el.Value))
                    {
                        serializedFieldsProp.DeleteArrayElementAtIndex(fieldRow.Value);
                    }
                    unrecognizedFields.Clear();
                    ClearContainer(unrecognizedFieldsContainer);
                    unrecognizedFieldsContainer.style.display = DisplayStyle.None;
                    serializedObject.ApplyModifiedProperties();
                }));

            var assemblyQualifiedName = inspectorDataContainer.Q<Label>("assemblyQualifiedName");
            assemblyQualifiedName.RegisterValueChangedCallback((evt) =>
            {
                EntityStateType = Type.GetType(evt.newValue);
            });

            //If the current type is not the targetType's targetType, update it, otherwise, re-populate the containers
            if (EntityStateType != (Type)TargetType.targetType)
            {
                EntityStateType = (Type)TargetType.targetType;
            }
            else
            {
                OnEntityStateTypeSet();
            }
        }

        private void OnEntityStateTypeSet()
        {
            PopulateSerializableFields();
            FindUnrecognizedFields();
            Debug.Log($"Found a total of {instanceSerializableFields.Count} serializable instance fields" +
    $"\nFound a total of {staticSerializableFields.Count} serializable static fields" +
    $"\nFound a total of {unrecognizedFields.Count} unrecognized fields");
            SetContainerFlex();
            serializedObject.ApplyModifiedProperties();
        }

        private void PopulateSerializableFields()
        {
            staticSerializableFields.Clear();
            ClearContainer(staticFieldsContainer);

            instanceSerializableFields.Clear();
            ClearContainer(instanceFieldsContainer);

            if (EntityStateType == null)
            {
                return;
            }

            var allFieldsInType = EntityStateType.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            var filteredFields = allFieldsInType.Where(fieldInfo =>
            {
                bool canSerialize = SerializedValue.CanSerializeField(fieldInfo);
                bool shouldSerialize = !fieldInfo.IsStatic || (fieldInfo.DeclaringType == EntityStateType);
                bool doesNotHaveAttribute = fieldInfo.GetCustomAttribute<HideInInspector>() == null;
                return canSerialize && shouldSerialize && doesNotHaveAttribute;
            });

            staticSerializableFields.AddRange(filteredFields.Where(fieldInfo => fieldInfo.IsStatic));
            instanceSerializableFields.AddRange(filteredFields.Where(fieldInfo => !fieldInfo.IsStatic));
        }

        private void FindUnrecognizedFields()
        {
            unrecognizedFields.Clear();
            ClearContainer(unrecognizedFieldsContainer);

            for (var i = 0; i < serializedFieldsProp.arraySize; i++)
            {
                var property = serializedFieldsProp.GetArrayElementAtIndex(i);
                var name = property.FindPropertyRelative(nameof(SerializedField.fieldName)).stringValue;
                if (!(staticSerializableFields.Any(el => el.Name == name) || instanceSerializableFields.Any(el => el.Name == name)))
                {
                    unrecognizedFields.Add(new KeyValuePair<SerializedProperty, int>(property, i));
                }
            }
        }
        private void SetContainerFlex()
        {
            staticFieldsContainer.style.display = staticSerializableFields.Count > 0 ? DisplayStyle.Flex : DisplayStyle.None;
            instanceFieldsContainer.style.display = instanceSerializableFields.Count > 0 ? DisplayStyle.Flex : DisplayStyle.None;
            unrecognizedFieldsContainer.style.display = unrecognizedFields.Count > 0 ? DisplayStyle.Flex : DisplayStyle.None;

            if (staticFieldsContainer.style.display == DisplayStyle.Flex)
            {
                //This ensures that the serializable static fields exists as serialized properties.
                EnsureFieldsExist(staticSerializableFields);
                DisplayFields(staticFieldsContainer, staticSerializableFields);
            }

            if (instanceFieldsContainer.style.display == DisplayStyle.Flex)
            {
                //This ensures that the serializable instance fields exists as serialized properties
                EnsureFieldsExist(instanceSerializableFields);
                DisplayFields(instanceFieldsContainer, instanceSerializableFields);
            }

            //Unrecognized fields already exist, no need to ensure their existence.
            if (unrecognizedFieldsContainer.style.display == DisplayStyle.Flex)
            {
                DisplayUnrecognizedFields(unrecognizedFieldsContainer, unrecognizedFields);
            }
        }

        private void DisplayUnrecognizedFields(VisualElement container, List<KeyValuePair<SerializedProperty, int>> unrecognizedFields)
        {
            foreach (var propertyRow in unrecognizedFields)
            {
                var name = propertyRow.Key.FindPropertyRelative(nameof(SerializedField.fieldName)).stringValue;
                var property = propertyRow.Key.FindPropertyRelative(nameof(SerializedField.fieldValue));

                var propertyField = new PropertyField(property, ObjectNames.NicifyVariableName(name));
                propertyField.name = $"{name}_Property";
                container.Add(propertyField);
                if (HasDoneFirstDrawing)
                {
                    propertyField.Bind(serializedObject);
                }
            }
        }
        private void EnsureFieldsExist(List<FieldInfo> fields)
        {
            foreach (FieldInfo fieldInfo in fields)
            {
                //If the field already exists, then it means the field either already has a value, or has already been initialized.
                bool fieldAlreadyExists = false;
                for (int i = 0; i < serializedFieldsProp.arraySize; i++)
                {
                    var prop = serializedFieldsProp.GetArrayElementAtIndex(i);

                    if (prop.FindPropertyRelative(nameof(SerializedField.fieldName)).stringValue == fieldInfo.Name)
                    {
                        fieldAlreadyExists = true;
                        break;
                    }
                }
                //Already exists? go to the next field.
                if (fieldAlreadyExists)
                    continue;
                //Otherwise, make new serialized property and initialize.
                serializedFieldsProp.arraySize++;
                var serializedFieldProp = serializedFieldsProp.GetArrayElementAtIndex(serializedFieldsProp.arraySize - 1);
                var fieldNameProp = serializedFieldProp.FindPropertyRelative(nameof(SerializedField.fieldName));
                fieldNameProp.stringValue = fieldInfo.Name;

                var fieldValueProp = serializedFieldProp.FindPropertyRelative(nameof(SerializedField.fieldValue));
                var serializedValue = new SerializedValue();

                if (specialDefaultValueCreators.TryGetValue(fieldInfo.FieldType, out var creator))
                {
                    serializedValue.SetValue(fieldInfo, creator());
                }
                else
                {
                    serializedValue.SetValue(fieldInfo, fieldInfo.FieldType.IsValueType ? Activator.CreateInstance(fieldInfo.FieldType) : (object)null);
                }

                fieldValueProp.FindPropertyRelative(nameof(SerializedValue.stringValue)).stringValue = serializedValue.stringValue;
                fieldValueProp.FindPropertyRelative(nameof(SerializedValue.objectValue)).objectReferenceValue = null;
            }
        }
        private void DisplayFields(VisualElement container, List<FieldInfo> fieldInfos)
        {
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                //If by any chance theres already a field in the container that has this name, dont add another one.
                if (container.Children().Any(child => child.name == $"{fieldInfo.Name}_Property"))
                    continue;

                SerializedProperty propertyForInfo = FindSerializedFieldProperty(fieldInfo.Name);
                VisualElement elementForProperty = GetVisualElementFromProperty(propertyForInfo, fieldInfo);
                container.Add(elementForProperty);
            }
        }

        private SerializedProperty FindSerializedFieldProperty(string fieldName)
        {
            for (int i = 0; i < serializedFieldsProp.arraySize; i++)
            {
                SerializedProperty prop = serializedFieldsProp.GetArrayElementAtIndex(i);
                if (prop.FindPropertyRelative("fieldName").stringValue == fieldName)
                {
                    return prop;
                }
            }
            return null;
        }

        private VisualElement GetVisualElementFromProperty(SerializedProperty property, FieldInfo fieldInfo)
        {
            var serializedValueProp = property.FindPropertyRelative(nameof(SerializedField.fieldValue));
            //If the type of the field is a unity object, just use an Object Field and set the binding path directly.
            if (typeof(UnityEngine.Object).IsAssignableFrom(fieldInfo.FieldType))
            {
                var objectValue = serializedValueProp.FindPropertyRelative(nameof(SerializedValue.objectValue));
                var objectField = new ObjectField();
                objectField.objectType = fieldInfo.FieldType;
                objectField.bindingPath = objectValue.propertyPath;
                objectField.name = $"{fieldInfo.Name}_Property";
                objectField.label = ObjectNames.NicifyVariableName(fieldInfo.Name);
                return objectField;
            }
            //Same applies for types that are string
            else if (fieldInfo.FieldType == typeof(string))
            {
                var stringValue = serializedValueProp.FindPropertyRelative(nameof(SerializedValue.stringValue));
                var textField = new TextField();
                textField.bindingPath = stringValue.propertyPath;
                textField.name = $"{fieldInfo.Name}_Property";
                textField.label = ObjectNames.NicifyVariableName(fieldInfo.Name);
                return textField;
            }
            else
            {
                //Otherwise, create a new element via the dictionary
                var stringValue = serializedValueProp.FindPropertyRelative(nameof(SerializedValue.stringValue));
                var serializedValue = new SerializedValue
                {
                    stringValue = string.IsNullOrWhiteSpace(stringValue.stringValue) ? null : stringValue.stringValue
                };

                if (drawers.TryGetValue(fieldInfo.FieldType, out var elementCreator))
                {
                    var value = serializedValue.GetValue(fieldInfo);
                    var element = elementCreator(fieldInfo, value);

                    //With the element created, we need to manually register value changes.
                    switch (value)
                    {
                        case bool _: SetupCallback<bool>(element); break;
                        case long _: SetupCallback<long>(element); break;
                        case int _: SetupCallback<int>(element); break;
                        case float _: SetupCallback<float>(element); break;
                        case double _: SetupCallback<double>(element); break;
                        case Vector2 _: SetupCallback<Vector2>(element); break;
                        case Vector3 _: SetupCallback<Vector3>(element); break;
                        case Color _: SetupCallback<Color>(element); break;
                        case Color32 _: SetupCallback<Color32>(element); break;
                        case AnimationCurve _: SetupCallback<AnimationCurve>(element); break;
                        default: Debug.LogWarning($"Could not setup callback for element {element} that's tied to field {fieldInfo.Name}"); break;
                    }
                    return element;
                }
            }
            //If the element is not in the dictionary, just make a property field
            var propertyField = new PropertyField(property, ObjectNames.NicifyVariableName(fieldInfo.Name));
            propertyField.name = $"{fieldInfo.Name}_Property";
            return propertyField;
        }

        private void SetupCallback<T>(VisualElement element)
        {
            BaseField<T> baseField = (BaseField<T>)element;
            baseField.RegisterValueChangedCallback(OnValueChanged);

            void OnValueChanged(ChangeEvent<T> evt)
            {
                BaseField<T> target = (BaseField<T>)evt.target;
                string fieldName = target.name.Replace("_Property", "");
                FieldInfo fieldInfo = EntityStateType.GetField(fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                ref SerializedField field = ref TargetType.serializedFieldsCollection.GetOrCreateField(fieldName);
                ref SerializedValue value = ref field.fieldValue;
                value.SetValue(fieldInfo, evt.newValue);
                serializedObject.Update();
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void ClearContainer(VisualElement element)
        {
            var label = element.Q<Label>();
            element.hierarchy.Clear();
            element.Add(label);
        }
    }
}