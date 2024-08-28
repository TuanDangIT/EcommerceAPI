using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Modules
{
    public interface IModuleSerializer
    {
        byte[] Serialize<T>(T value);
        T Deserialize<T>(byte[] value);
        object Deserialize(byte[] value, Type type);
    }
}
