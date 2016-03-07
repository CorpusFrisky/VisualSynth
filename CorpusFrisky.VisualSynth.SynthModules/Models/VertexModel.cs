using System.Collections.Generic;
using System.Linq;
using CorpusFrisky.VisualSynth.SynthModules.Interfaces;
using CorpusFrisky.VisualSynth.SynthModules.Models.Enums;
using CorpusFrisky.VisualSynth.SynthModules.Models.Modifiers;
using Microsoft.Practices.Prism.Mvvm;
using OpenTK;
using OpenTK.Graphics;

namespace CorpusFrisky.VisualSynth.SynthModules.Models
{
    public class VertexModel : BindableBase
    {
        #region fields

        private Vector3 _position;
        private Color4 _color;
        private readonly Dictionary<PinTagetPropertyEnum, List<IPropertyModifierModule>> _propertyModifiers;
        private Vector3 _modifiedPosition;

        #endregion

        public VertexModel()
        {
            _propertyModifiers = new Dictionary<PinTagetPropertyEnum, List<IPropertyModifierModule>>();
        }

        #region Properties

        public Vector3 Position
        {
            get { return _position; }
            set
            {
                SetProperty(ref _position, value);
                ModifiedPosition = new Vector3(_position);
            }
        }

        public Color4 Color
        {
            get { return _color; }
            set
            {
                SetProperty(ref _color, value);
                ModifiedColor = new Color4(_color.R, _color.G, _color.B, _color.A);
            }
        }

        public Vector3 ModifiedPosition { get; set; }

        public Color4 ModifiedColor { get; set; }

        #endregion

        #region Methods

        public void UpdateVertex()
        {
            ApplyModifiers();
        }

        public void AddPropertyModifier(PinTagetPropertyEnum property, IModifierModule module)
        {
            var propertyModifier = module as IPropertyModifierModule;
            if (propertyModifier == null)
            {
                //TODO: log
                return;
            }

            if (!_propertyModifiers.ContainsKey(property))
            {
                _propertyModifiers.Add(property, new List<IPropertyModifierModule>());
            }

            _propertyModifiers[property].Add(propertyModifier);
        }

        public void RemovePropertyModifier(PinTagetPropertyEnum property, IModifierModule module)
        {
            if (_propertyModifiers.ContainsKey(property))
            {
                var moduleToRemove = _propertyModifiers[property].First(x => x == module);
                if (moduleToRemove != null)
                {
                    _propertyModifiers[property].Remove(moduleToRemove);
                }
            }
        }

        public void ApplyModifiers()
        {
            if (_propertyModifiers.ContainsKey(PinTagetPropertyEnum.Position))
            {
                ApplyPositionModifiers();                
            }

            if (_propertyModifiers.ContainsKey(PinTagetPropertyEnum.Color))
            {
                ApplyColorModifiers();
            }
        }

        private void ApplyColorModifiers()
        {
            var modificationValue = _propertyModifiers[PinTagetPropertyEnum.Color].Sum(modifier => modifier.GetValue());
            modificationValue /= _propertyModifiers[PinTagetPropertyEnum.Color].Count;

            //Use the set color value as a max and oscillate around the midpoint between the max and 0;
            //  For red:  (r/2) + (r/2)*modValue  --->   (r/2)*(1 + modValue)   ---->  r * ((1 + mod)/2)
            var onePlusModValueOver2 = 0.5*(1 + modificationValue);
            ModifiedColor = new Color4((float)(Color.R * onePlusModValueOver2),
                   (float)(Color.G * onePlusModValueOver2),
                   (float)(Color.B * onePlusModValueOver2),
                   1.0f);
        }

        private void ApplyPositionModifiers()
        {
            //foreach (var modifier in _propertyModifiers[VertexProperty.Position])
            //{
            //    Position = modifier.Apply(Position);
            //}
        }

        #endregion
    }
}