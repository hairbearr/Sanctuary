using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sanctuary.Harry.Core
{
    public interface IPredicateEvaluator
    {
        //? after a bool turns it into a nullable boolean, where you can set it to null
        bool? Evaluate(string predicate, string[] parameters);
    }
}
