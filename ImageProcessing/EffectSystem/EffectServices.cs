using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing.EffectSystem
{
    public class EffectService
    {
        readonly List<EffectAssembly> _effectAssemblies;
        public EffectService() 
        {
            _effectAssemblies = new List<EffectAssembly>();
        }
        public IReadOnlyList<EffectAssembly> EffectAssemblies { get => _effectAssemblies; }
        public IReadOnlyList<ImageProcessorUnit> AllImageProcessorUnits
        {
            get
            {
                List<ImageProcessorUnit> imageProcessorUnits = new List<ImageProcessorUnit>();
                _effectAssemblies.ForEach(x => imageProcessorUnits.AddRange(x.ImageProcessorUnits));
                return imageProcessorUnits;
            }
        }
        public void Add(EffectAssembly assembly)
        {
            _effectAssemblies.Add(assembly);
            OnEffectsChanged?.Invoke(this,EventArgs.Empty);
        }
        public void Remove(EffectAssembly assembly)
        {
            _effectAssemblies.Remove(assembly);
            OnEffectsChanged?.Invoke(this,EventArgs.Empty);
        }

        public event Action<object, EventArgs> OnEffectsChanged;
    }
}
