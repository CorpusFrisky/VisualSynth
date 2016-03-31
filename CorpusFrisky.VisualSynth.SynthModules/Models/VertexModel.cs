using System.Collections.Generic;
using System.Linq;
using CorpusFrisky.VisualSynth.SynthModules.Interfaces;
using CorpusFrisky.VisualSynth.SynthModules.Models.Enums;
using CorpusFrisky.VisualSynth.SynthModules.Models.Pins;
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
        private readonly Dictionary<PinTagetPropertyEnum, List<OutputValuePin>> _propertyModifiers;
        private Vector3 _modifiedPosition;

        #endregion

        public VertexModel()
        {
            _propertyModifiers = new Dictionary<PinTagetPropertyEnum, List<OutputValuePin>>();
        }

        #region Properties

        public Vector3 Position
        {
            get { return _position; }
            set
            {
                SetProperty(ref _position, value);
                ApplyModifiers();
            }
        }

        public Color4 Color
        {
            get { return _color; }
            set
            {
                SetProperty(ref _color, value);
                ApplyModifiers();
            }
        }

        public float X
        {
            get { return Position.X; }
            set
            {
                _position.X = value;
                OnPropertyChanged("Position");
            }
        }

        public float Y
        {
            get { return Position.Y; }
            set
            {
                _position.Y = value;
                OnPropertyChanged("Position");
            }
        }

        public float Z
        {
            get { return Position.Z; }
            set
            {
                _position.Z = value;
                OnPropertyChanged("Position");
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

        public void AddPropertyModifier(PinTagetPropertyEnum property, OutputValuePin pin)
        {
            if (!_propertyModifiers.ContainsKey(property))
            {
                _propertyModifiers.Add(property, new List<OutputValuePin>());
            }

            _propertyModifiers[property].Add(pin);
        }

        public void RemovePropertyModifier(PinTagetPropertyEnum property, OutputValuePin pin)
        {
            if (_propertyModifiers.ContainsKey(property))
            {
                var moduleToRemove = _propertyModifiers[property].First(x => x == pin);
                if (moduleToRemove != null)
                {
                    _propertyModifiers[property].Remove(moduleToRemove);
                }
            }
        }

        public void ApplyModifiers()
        {
            if (_propertyModifiers.ContainsKey(PinTagetPropertyEnum.Position) &&
                _propertyModifiers[PinTagetPropertyEnum.Position].Any())
            {
                ApplyPositionModifiers();                
            }
            else
            {
                ModifiedPosition = Position;
            }

            if (_propertyModifiers.ContainsKey(PinTagetPropertyEnum.Color) &&
                _propertyModifiers[PinTagetPropertyEnum.Color].Any())
            {
                ApplyColorModifiers();
            }
            else
            {
                ModifiedColor = Color;
            }
        }

        private void ApplyColorModifiers()
        {
            var modificationValue = _propertyModifiers[PinTagetPropertyEnum.Color].Sum(modifier => modifier.GetValue_Function());
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