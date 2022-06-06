using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace RoR2EditorKit.Core.Inspectors
{
    /// <summary>
    /// A wrapper for ListView for ease of use.
    /// <para>The wrapper allows the end user to create a Listview that automatically binds to children in the property given by its constructor.</para>
    /// <para>Unlike normally setting the Listview's bindingPath and relying on that, the ListViewHelper allows for extra modification of the elements created and bound.</para>
    /// <para>ListViewHelper also always ensures the ListView's style height is never 0</para>
    /// <para>For usage, look at RoR2EK's NetworkStateMachineInspector</para>
    /// </summary>
    public class ListViewHelper
    {
        /// <summary>
        /// Data for initializing a ListViewHelper
        /// </summary>
        public struct ListViewHelperData
        {
            public SerializedProperty property;
            public ListView listView;
            public IntegerField intField;
            public Action<VisualElement, SerializedProperty> bindElement;
            public Func<VisualElement> createElement;

            /// <summary>
            /// ListViewHelperData Constructor
            /// </summary>
            /// <param name="sp">The SerializedProperty thats going to be displayed using the ListView, Property must be an Array Property</param>
            /// <param name="lv">The ListView element</param>
            /// <param name="intfld">An IntegerField that's used for modifying the SerializedProperty's ArraySize</param>
            /// <param name="crtItem">Function for creating a new Element to display</param>
            /// <param name="bnd">Action for binding the SerializedProperty to the VisualElement, there is no need to call Bind() on any elements, as the ListViewHelper takes care of it.</param>
            public ListViewHelperData(SerializedProperty sp, ListView lv, IntegerField intfld, Func<VisualElement> crtItem, Action<VisualElement, SerializedProperty> bnd)
            {
                property = sp;
                listView = lv;
                intField = intfld;
                bindElement = bnd;
                createElement = crtItem;
            }
        }

        /// <summary>
        /// The SerializedObject that owns the <see cref="SerializedProperty"/>
        /// </summary>
        public SerializedObject SerializedObject { get => _serializedObject; }
        private SerializedObject _serializedObject;
        /// <summary>
        /// The SerializedProperty thats being used for the ListView
        /// </summary>
        public SerializedProperty SerializedProperty
        {
            get => _serializedProperty;
            set
            {
                if (_serializedProperty != value)
                {
                    _serializedProperty = value;
                    _serializedObject = value.serializedObject;
                    SetupArraySize();
                    SetupListView();
                }
            }
        }
        private SerializedProperty _serializedProperty;
        /// <summary>
        /// The ListView element
        /// </summary>
        public ListView TiedListView { get; }
        /// <summary>
        /// An IntegerField thats used for modifying the <see cref="SerializedProperty"/>'s ArraySize
        /// </summary>
        public IntegerField ArraySize { get; }
        /// <summary>
        /// The Action for Binding a VisualElement
        /// </summary>
        public Action<VisualElement, SerializedProperty> BindElement { get; }
        /// <summary>
        /// The Function for creating the VisualElement
        /// </summary>
        public Func<VisualElement> CreateElement { get; }

        /// <summary>
        /// ListViewHelper Constructor
        /// </summary>
        /// <param name="data">The Data for constructiong the ListView</param>
        public ListViewHelper(ListViewHelperData data)
        {
            if (data.property != null)
            {
                _serializedProperty = data.property;
                _serializedObject = SerializedProperty.serializedObject;
            }
            TiedListView = data.listView;
            ArraySize = data.intField;
            BindElement = data.bindElement;
            CreateElement = data.createElement;

            SetupArraySize();
            SetupListView();
        }

        /// <summary>
        /// Refreshes the listview helper by setting the serialized property's array size to itself's size
        /// This causes a chain reaction that redraws the entirety of the elements tied to the helper
        /// </summary>
        public void Refresh()
        {
            OnSizeSetInternal(SerializedProperty == null ? 0 : SerializedProperty.arraySize);
        }

        private void SetupArraySize()
        {
            ArraySize.value = SerializedProperty == null ? 0 : SerializedProperty.arraySize;
            ArraySize.isDelayed = true;
            ArraySize.RegisterValueChangedCallback(OnSizeSet);

            void OnSizeSet(ChangeEvent<int> evt)
            {
                int value = evt.newValue < 0 ? 0 : evt.newValue;
                OnSizeSetInternal(value);
            }
        }

        private void OnSizeSetInternal(int newSize)
        {
            ArraySize.value = newSize;
            if (SerializedProperty != null)
                SerializedProperty.arraySize = newSize;
            TiedListView.itemsSource = new int[newSize];
            SerializedObject?.ApplyModifiedProperties();
        }
        private void SetupListView()
        {
            TiedListView.itemsSource = SerializedProperty == null ? Array.Empty<int>() : new int[SerializedProperty.arraySize];

            TiedListView.makeItem = CreateElement;
            TiedListView.bindItem = BindItemInternal;
        }
        private void BindItemInternal(VisualElement ve, int i)
        {
            SerializedProperty propForElement = SerializedProperty.GetArrayElementAtIndex(i);
            ve.name = $"element{i}";
            BindElement(ve, propForElement);
            ve.Bind(SerializedObject);
        }
    }
}
