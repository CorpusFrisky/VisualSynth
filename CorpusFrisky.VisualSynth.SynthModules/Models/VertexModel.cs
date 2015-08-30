using System.Collections.Generic;
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
        private readonly Dictionary<VertexProperty, List<IPropertyModifierModule>> _propertyModifiers;
        private Vector3 _modifiedPosition;

        #endregion

        public VertexModel()
        {
            _propertyModifiers = new Dictionary<VertexProperty, List<IPropertyModifierModule>>();
        }

        #region Property Enum
        
        public enum VertexProperty
        {
            Position = 0,
            Color,
        };

        #endregion

        #region Properties

        public Vector3 Position
        {
            get { return _position; }
            set { SetProperty(ref _position, value); }
        }

        public Color4 Color
        {
            get { return _color; }
            set { SetProperty(ref _color, value); }
        }

        public Vector3 ModifiedPosition { get; set; }

        public Color4 ModifiedColor { get; set; }

        #endregion

        #region Methods

        public void UpdateVertex()
        {
            ApplyModifiers();
        }

        public void AddPropertyModifier(VertexProperty property, IModifierModule module)
        {
            if (!_propertyModifiers.ContainsKey(property))
            {
                _propertyModifiers.Add(property, new List<IPropertyModifierModule>());
            }
        }

        public void ApplyModifiers()
        {
            if (_propertyModifiers.ContainsKey(VertexProperty.Position))
            {
                ApplyPositionModifiers();                
            }

            ApplyColorModifiers();
        }

        private void ApplyColorModifiers()
        {
            foreach (var modifier in _propertyModifiers[VertexProperty.Color])
            {
                ModifiedColor = new Color4((float)(Color.R*modifier.GetValue()),
                    (float)(Color.G * modifier.GetValue()),
                    (float)(Color.B * modifier.GetValue()),
                    1.0f);
            }
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