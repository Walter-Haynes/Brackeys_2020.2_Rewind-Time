using System.Collections.Generic;

using UnityEngine;

using JetBrains.Annotations;

namespace Core.Player.GenericAbilitySystem
{
    using System;
    using Sirenix.OdinInspector;
    
    using CommonGames.Utilities.Extensions;
    using CommonGames.Utilities.Helpers;
    using Sirenix.Serialization;

    /// <summary> Contains references to its shared <see cref="AbilitySystemBehaviour{TCore}"/>s and manages the system's Abilities. </summary>
    /// <typeparam name="TCore"></typeparam>
    public abstract class AbilitySystemCore<TCore> : MonoBehaviour
        where TCore : AbilitySystemCore<TCore>
    {
        #region Variables

        [SerializeField] 
        [OdinSerialize] private List< AbilitySystemBehaviour<TCore> > _abilitySystemBehaviours = new List<AbilitySystemBehaviour<TCore>>();
        [PublicAPI]
        public List< AbilitySystemBehaviour<TCore> > AbilitySystemBehaviours
        {
            get => _abilitySystemBehaviours;
            protected set => _abilitySystemBehaviours = value;
        }

        //TODO -Walter-: Add Generic State Machine. 
        //[PublicAPI]
        //public CharacterState CurrentCharacterState { get; private set; } = CharacterState.Default;

        #endregion

        #region Methods

        #region Unity Event Functions

        /// <summary> Gather the attached <see cref="AbilitySystemBehaviour{TCore}"/> components On Enable. </summary>
        protected virtual void OnEnable()
        {
            if(PrefabCheckHelper.CheckIfPrefab(component: this)) return;

            GatherSystemBehaviours();
        }

        #endregion

        #region Initialization

        /// <summary> Finds all new <see cref="AbilitySystemBehaviour{TCore}"/> Components on this object and its children,
        /// and adds them to the <see cref="AbilitySystemBehaviours"/> List. </summary>
        [ContextMenu(itemName: "Gather Ability System Behaviours")]
        protected virtual void GatherSystemBehaviours()
        {
            AbilitySystemBehaviour<TCore>[] __componentsInChildren = GetComponentsInChildren< AbilitySystemBehaviour<TCore> >();

            List< AbilitySystemBehaviour<TCore> > __newComponents = new List< AbilitySystemBehaviour<TCore> >(capacity: __componentsInChildren.Length);
                
            foreach( AbilitySystemBehaviour<TCore>  __systemBehaviour in __componentsInChildren)
            {
                if(_abilitySystemBehaviours.Contains(__systemBehaviour)) continue;
                    
                __newComponents.Add(__systemBehaviour);
            }

            if(__newComponents.Count >= 1)
            {
                _abilitySystemBehaviours.CGAdd(__newComponents);   
            }

            SetUpSystemBehaviours();
        }

        /// <summary> Sets up reference to this <see cref="AbilitySystemCore{TCore}"/> in childed <see cref="AbilitySystemBehaviour{TCore}"/>s. </summary>
        //[Button]
        protected virtual void SetUpSystemBehaviours()
        {
            if(AbilitySystemBehaviours == null) return;
            
            if(AbilitySystemBehaviours.Count < 1) return;
            
            foreach(AbilitySystemBehaviour<TCore> __characterBehaviour in AbilitySystemBehaviours)
            {
                __characterBehaviour.SetSystemCoreReference(systemCore: this as TCore);
            }
        }
        
        #endregion

        [PublicAPI]
        public T GetBehaviour<T>()
            where T : AbilitySystemBehaviour<TCore>
            => AbilitySystemBehaviours.Get(func: ability => ability is T) as T;

        #endregion
    }
}
