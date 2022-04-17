using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sanctuary.Harry.Core
{
    [System.Serializable]
    public class Condition
    {
        [SerializeField]
        Disjunction[] and;

        public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
        {
            foreach (Disjunction dis in and)
            {
                if (!dis.Check(evaluators)) { return false; }
            }
            return true;
        }

        [System.Serializable]
        class Disjunction
        {
            [SerializeField] Predicate[] or;

            public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach (Predicate pred in or)
                {
                    if (pred.Check(evaluators)) { return true; }
                }
                return false;
            }
        }

        [System.Serializable]
        class Predicate
        {
            [SerializeField] string predicate;
            [SerializeField] string[] parameters;
            [SerializeField] bool negate = false;

            public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
            {
                //Debug.Log($"Testing Predicate {predicate}({parameters[0]})");

                foreach (var evaluator in evaluators)
                {

                    //Debug.Log($"{evaluator} is evaluating {predicate}");

                    bool? result = evaluator.Evaluate(predicate, parameters);

                    if (result == null)
                    {

                        //Debug.Log($"{evaluator} cannot handle {predicate}");
                        continue;
                    
                    }

                    if (result == negate)
                    {
                        //Debug.Log($"result = {result}, but expecting {!negate} returning false");
                        return false;
                    }
                }
                return true;
            }
        }
    }
}