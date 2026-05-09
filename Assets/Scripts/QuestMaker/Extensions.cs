using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class TypeExtension
{
    public static IEnumerable<Type> GetInterfacesOfType(this Type lookupType, Type lookupInterface)
            =>lookupType.GetInterfaces().Where(i => lookupInterface.IsAssignableFrom(i)).ToList();
    
}

