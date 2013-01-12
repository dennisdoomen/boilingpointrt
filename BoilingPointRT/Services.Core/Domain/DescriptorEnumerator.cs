using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BoilingPointRT.Services.Domain
{
    public class DescriptorEnumerator<TDescriptor> : IDescriptorEnumerator<TDescriptor> where TDescriptor : Descriptor, new()
    {
        /// <summary>
        /// Gets all possible descriptors values for <typeparamref name="TDescriptor"/>.
        /// </summary>
        public IEnumerable<TDescriptor> GetAll()
        {
            Type type = typeof(TDescriptor);
            var descriptors = new List<TDescriptor>();

            while (type != typeof(Descriptor))
            {
                foreach (var item in GetDescriptorsFor(type))
                {
                    if (!descriptors.Contains(item))
                    {
                        if (item is TDescriptor)
                        {
                            descriptors.Add((TDescriptor)item);
                        }
                        else
                        {
                            descriptors.Add(new TDescriptor { Code = item.Code });
                        }
                    }
                }

                type = type.BaseType;
            }

            return descriptors;
        }

        private static IEnumerable<Descriptor> GetDescriptorsFor(Type type)
        {
            IEnumerable<FieldInfo> descriptorFields = GetStaticFields(type);
            return descriptorFields.Select(f => f.GetValue(null)).Cast<Descriptor>();
        }

        private static IEnumerable<FieldInfo> GetStaticFields(Type type)
        {
            return type.GetFields(BindingFlags.Static | BindingFlags.Public);
        }
    }
}