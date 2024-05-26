using Assets.Scripts.Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Manager
{
    public class CharacterManager : Singleton<CharacterManager>, IDisposable
    {

        public Dictionary<int, Character> Characters = new Dictionary<int, Character>();


        public UnityAction<Character> OnCharacterEnter;
        public UnityAction<Character> OnCharacterLeave;

        public CharacterManager()
        {

        }

        public void Dispose()
        {
        }

        public void Init()
        {

        }

        public void Clear()
        {
            int[] keys = this.Characters.Keys.ToArray();
            foreach (var key in keys)
            {
                this.RemoveCharacter(key);
            }
            this.Characters.Clear();
        }

        public void AddCharacter(SkillBridge.Message.NCharacterInfo cha)
        {
            Debug.LogFormat("AddCharacter:{0}:{1} Map:{2} Entity:{3}", cha.Id, cha.Name, cha.mapId, cha.Entity.String());
            Character character = new Character(cha);
            this.Characters[cha.Id] = character;
            EntityManager.Instance.AddEntity(character);
            character.Info.Id = character.Id;
            if (OnCharacterEnter != null)
            {
                OnCharacterEnter(character);
            }
        }


        public void RemoveCharacter(int Id)
        {
            Debug.LogFormat("RemoveCharacter:{0}", Id);
            if (this.Characters.ContainsKey(Id))
            {
                EntityManager.Instance.RemoveEntity(this.Characters[Id].Info.Entity);
                if (OnCharacterLeave != null)
                {
                    OnCharacterLeave(this.Characters[Id]);
                }
                this.Characters.Remove(Id);
            }
        }

        public Character GetCharacter(int id)
        {
            Character character;
            this.Characters.TryGetValue(id, out character);
            return character;
        }
    }
}
