using Antura.Core;
using UnityEngine;

namespace Antura.Map
{
    public interface IMapLocation
    {
        Vector3 Position { get; }
        JourneyPosition JourneyPos { get; }
    }
}