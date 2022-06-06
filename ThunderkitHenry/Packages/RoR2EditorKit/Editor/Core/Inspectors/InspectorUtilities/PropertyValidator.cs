using RoR2EditorKit.Utilities;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace RoR2EditorKit.Core.Inspectors
{
    /// <summary>
    /// Class for creating validation methods for PropertyFields or VisualElements that implement INotifyValueChanged.
    /// </summary>
    /// <typeparam name="T">The type that's used for the change event</typeparam>
    public class PropertyValidator<T>
    {
        /// <summary>
        /// The type of the TiedElement
        /// </summary>
        public enum TiedElementType
        {
            PropertyField,
            INotifyValueChanged,
        }
        /// <summary>
        /// A pair of an Action for drawing the HelpBox, and the IMGUI container for it.
        /// </summary>
        public class ActionContainerPair
        {
            public Action action;
            public IMGUIContainer container;
        }

        /// <summary>
        /// The element that's tied to this PropertyValidator
        /// </summary>
        public VisualElement TiedElement { get; }
        /// <summary>
        /// The type of element, if its <see cref="TiedElementType.PropertyField", the <see cref="TiedElement"/> is a property field/>
        /// <para>otherwise, its a Visual Element that implements INotifyValueChanged</para>
        /// </summary>
        public TiedElementType TypeOfTiedElement { get; }
        /// <summary>
        /// The VisualElement where the HelpBoxes will be attached to
        /// </summary>
        public VisualElement ParentElement { get; }
        /// <summary>
        /// Retrieves the latest ChangeEvent that has ran for this PropertyValidator, can be null.
        /// </summary>
        public ChangeEvent<T> ChangeEvent { get => _changeEvent; }
        private ChangeEvent<T> _changeEvent;

        private Dictionary<Func<bool?>, ActionContainerPair> validatorToMessageAction = new Dictionary<Func<bool?>, ActionContainerPair>();

        /// <summary>
        /// PropertyValidator constructor
        /// </summary>
        /// <param name="propField">The propertyField that's going to be validated</param>
        /// <param name="parentElementToAttach">The element where the HelpBoxes will be attached to.</param>
        public PropertyValidator(PropertyField propField, VisualElement parentElementToAttach)
        {
            TiedElement = propField;
            TypeOfTiedElement = TiedElementType.PropertyField;
            ParentElement = parentElementToAttach;
            TiedElement.RegisterCallback<ChangeEvent<T>>(ValidateInternal);
        }

        /// <summary>
        /// PropertyValidator constructor
        /// </summary>
        /// <param name="valueChangedInterface">A VisualElement that implements INotifyValueChanged, this is going to be the element thats going to be validated</param>
        /// <param name="parentElementToAttach">The element where the HelpBoxes will be attached to.</param>
        public PropertyValidator(INotifyValueChanged<T> valueChangedInterface, VisualElement parentElementToAttach)
        {
            TiedElement = valueChangedInterface as VisualElement;
            TypeOfTiedElement = TiedElementType.INotifyValueChanged;
            ParentElement = parentElementToAttach;
            (TiedElement as INotifyValueChanged<T>).RegisterValueChangedCallback(ValidateInternal);
        }

        /// <summary>
        /// Adds a new Validator to the PropeprtyValidator
        /// </summary>
        /// <param name="condition">The condition that must happen for the MessageBox to appear. if the returned bool is null, it'll skip the rest of the validation logic.</param>
        /// <param name="message">The message of the MessageBox</param>
        /// <param name="messageType">The type of Message</param>
        public void AddValidator(Func<bool?> condition, string message, MessageType messageType = MessageType.Info)
        {
            validatorToMessageAction.Add(condition, new ActionContainerPair
            {
                action = new Action(() => EditorGUILayout.HelpBox(message, messageType)),
                container = null
            });
        }

        /// <summary>
        /// Forces the validation to run, regardless if there's a changeEvent or not.
        /// </summary>
        public void ForceValidation() => ValidateInternal(null);
        private void ValidateInternal(ChangeEvent<T> evt)
        {
            _changeEvent = evt;
            foreach (var (validator, actionContainerPair) in validatorToMessageAction)
            {
                bool? value = validator();
                if (value == null)
                {
                    continue;
                }

                if (value.Value)
                {
                    if (actionContainerPair.container == null)
                    {
                        actionContainerPair.container = new IMGUIContainer(actionContainerPair.action);
                        ParentElement.Add(actionContainerPair.container);
                        actionContainerPair.container.BringToFront();
                        continue;
                    }
                    actionContainerPair.container.BringToFront();
                }
                if (actionContainerPair.container != null)
                {
                    actionContainerPair.container.Wipe();
                    actionContainerPair.container.RemoveFromHierarchy();
                    actionContainerPair.container = null;
                }
            }
        }
    }
}