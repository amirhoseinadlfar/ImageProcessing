using ImageProcessing.Base.Attributes;
using ImageProcessing.Base.Effects;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing.EffectSystem
{
    public class EffectAssembly
    {
        public EffectAssembly(Assembly assembly)
        {
            var attribute = assembly.GetCustomAttribute<EffectAssemblyInfoAttribute>()!;
            if (attribute != null)
            {
                Name = string.IsNullOrWhiteSpace(attribute.Name) ? assembly.FullName! : attribute.Name;
                Description = attribute.Description;
                Creator = attribute.Creator;
            }
            else
            {
                Name = assembly.FullName!;
                Description = string.Empty;
                Creator = string.Empty;
            }
            Type[] processors = assembly.GetExportedTypes().Where(x => x.GetInterfaces().Any(interfac => interfac.GetGenericTypeDefinition() == typeof(IEffectBase<>))).ToArray();
            ImageProcessorUnits = new ImageProcessorUnit[processors.Length];
            for (int i = 0; i < processors.Length; i++)
            {
                ImageProcessorUnits[i] = new ImageProcessorUnit(processors[i],this);
            }
            PixelImageProcessors = ImageProcessorUnits.Where(x => x.EffectType == EffectType.Pixel).ToArray();
            AreaImageProcessors = ImageProcessorUnits.Where(x => x.EffectType == EffectType.Area).ToArray();
        }
        public string Name { get; }
        public string Description { get; }
        public string Creator { get; }

        public ImageProcessorUnit[] AreaImageProcessors { get; }
        public ImageProcessorUnit[] PixelImageProcessors { get; }
        public ImageProcessorUnit[] ImageProcessorUnits { get; }
    }
}
