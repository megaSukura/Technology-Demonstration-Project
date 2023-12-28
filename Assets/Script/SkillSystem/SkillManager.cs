using UnityEngine;
using System.Collections.Generic;
using System;
namespace SkillSystem
{
    [RequireComponent(typeof(GameEntity))]
    public class SkillManager : MonoBehaviour
    {
        public GameEntity entity;
        //
        private List<Skill> skills = new List<Skill>();
        
        public int SkillCount;
        public int AlwaysSceduleLineCount;
        private Dictionary<string, Skill> skillDict = new Dictionary<string, Skill>();

        public Skill Add(Skill skill)
        {
            skills.Add(skill);
            if(skillDict.ContainsKey(skill.SkillName))
            {
                Debug.LogError("SkillManager.Add: SkillName重复");
                return null;
            }
            skillDict.Add(skill.SkillName, skill);
            skill.manager = this;
            return skill;
        }
        public void Remove(Skill skill)
        {
            skill.End();
            skills.Remove(skill);
            skillDict.Remove(skill.SkillName);
        }
        public void ClearAll()
        {
            foreach (Skill skill in skills)
            {
                skill.End();
            }
            skills.Clear();
            skillDict.Clear();
            AlwaysSceduleLines.Clear();
        }
        public Skill this[string skillName]
        {
            get
            {
                return skillDict[skillName];
            }
        }

        //Awlways SceduleLine
        public List<ScheduleLine> AlwaysSceduleLines = new List<ScheduleLine>(); 
        public ScheduleLine ConnectAlways(Skill to , Func<bool> trigger,bool isResetTargetReady=false)
        {
            to.manager = this;
            ScheduleLine line = new ScheduleLine(null, to, trigger, isResetTargetReady, false);
            AlwaysSceduleLines.Add(line);
            return line;
        }
        public ScheduleLine ConnectAlways(Skill to, Trigger trigger, bool isResetTargetReady = false)
        {   
            to.manager = this;
            ScheduleLine line = new ScheduleLine(null, to, trigger, isResetTargetReady, false);
            AlwaysSceduleLines.Add(line);
            return line;
        }
        
#region 共用parameters
            //基础
            public Dictionary<string,Parameter<float>> floatParameters = new Dictionary<string, Parameter<float>>();
            public Dictionary<string,Parameter<int>> intParameters = new Dictionary<string, Parameter<int>>();
            public Dictionary<string,Parameter<bool>> boolParameters = new Dictionary<string, Parameter<bool>>();
            public Dictionary<string,Parameter<Vector2>> vector2Parameters = new Dictionary<string, Parameter<Vector2>>();
            public Parameter<T> GetParameter<T>(string name) where T : struct
            {
                if (typeof(T) == typeof(float))
                {
                    return floatParameters[name] as Parameter<T>;
                }
                else if (typeof(T) == typeof(int))
                {
                    return intParameters[name] as Parameter<T>;
                }
                else if (typeof(T) == typeof(bool))
                {
                    return boolParameters[name] as Parameter<T>;
                }
                else if (typeof(T) == typeof(Vector2))
                {
                    return vector2Parameters[name] as Parameter<T>;
                }
                else
                {
                    Debug.LogError("Parameter type not supported");
                    return null;
                }
            }
            public void AddParameter<T>(string name, T defaultValue,Func<T> agentGetter=null) where T : struct
            {
                if (typeof(T) == typeof(float))
                {
                    floatParameters.Add(name, new Parameter<float>((float)(object)defaultValue, agentGetter as Func<float>));
                }
                else if (typeof(T) == typeof(int))
                {
                    intParameters.Add(name, new Parameter<int>((int)(object)defaultValue, agentGetter as Func<int>));
                }
                else if (typeof(T) == typeof(bool))
                {
                    boolParameters.Add(name, new Parameter<bool>((bool)(object)defaultValue, agentGetter as Func<bool>));
                }
                else if (typeof(T) == typeof(Vector2))
                {
                    vector2Parameters.Add(name, new Parameter<Vector2>((Vector2)(object)defaultValue, agentGetter as Func<Vector2>));
                }
                else
                {
                    Debug.LogError("Parameter type not supported");
                }
            }
            
            //常用参数
            public Parameter<Vector2> Direction
            {
                get
                {
                    if(!vector2Parameters.ContainsKey("Direction"))
                    {
                        AddParameter<Vector2>("Direction", Vector2.zero);
                    }
                    return vector2Parameters["Direction"];
                }
            }
#endregion
        void UpdateActiveSkills()
        {   
            foreach (Skill skill in skills)
            {
                
                if (skill.isActivated)
                {   
                    
                    skill.Update();
                }
            }
        }
        void UpdateAlwaysScheduleLines()
        {
            foreach (ScheduleLine line in AlwaysSceduleLines)
            {
                line.Update();
            }
        }
        private void Awake() {
            entity = GetComponent<GameEntity>();
        }
        public bool IsEditorMode = false;
        void Update()
        {
            if(IsEditorMode)
                return;
            UpdateActiveSkills();
            UpdateAlwaysScheduleLines();
            //debug
            SkillCount = skills.Count;
            AlwaysSceduleLineCount = AlwaysSceduleLines.Count;
        }
    }
}
