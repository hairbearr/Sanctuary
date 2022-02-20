using System.Collections.Generic;

namespace Sanctuary.Harry.Stats
{
    public interface IModifierProvider
    {
        IEnumerable<float> GetAdditiveMods(Stat stat);
        IEnumerable<float> GetPercentageMods(Stat stat);
    }
}
