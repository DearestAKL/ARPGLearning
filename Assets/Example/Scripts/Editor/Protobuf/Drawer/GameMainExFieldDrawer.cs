using System;
using System.Collections.Generic;
using System.Linq;
using Akari.GfCore;
using Akari.GfUnityEditor.ProtobufExtensions;
using GameMain.Runtime;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using UnityEditor;
using UnityEngine;

namespace GameMain.Editor
{
    public class GameMainExFieldDrawer : DefaultFieldDrawer
    {
        protected static readonly Dictionary<int, string> ReactionLevelTypeDisplayStrings = new Dictionary<int, string>()
        {
            {(int)ReactionLevelType.None, "无"},
            {(int)ReactionLevelType.Tremor, "微颤"},
            {(int)ReactionLevelType.LightHit, "轻击"},
            {(int)ReactionLevelType.KnockBack, "击退"},
            {(int)ReactionLevelType.KnockUp, "击飞"},
        };
        
        protected static readonly Dictionary<int, string> BufferTypeDisplayStrings = new Dictionary<int, string>()
        {
            {(int)BufferEffectType.Attribute, "改变属性-根据基础数值与来源控制"},
            {(int)BufferEffectType.ChangeCurHp, "改变当前生命值(瞬间)"},
            {(int)BufferEffectType.AttributeByHealthLost, "改变属性-根据已损失生命值百分比控制"},
        };
        
        protected static readonly Dictionary<int, string> BufferEffectTriggerTypeDisplayStrings = new Dictionary<int, string>()
        {
            {(int)BufferEffectTriggerType.OnUpdate, "持续类型"},
            {(int)BufferEffectTriggerType.OnBegin, "buffer开始时"},
            {(int)BufferEffectTriggerType.OnEnd, "buffer结束时"},
            {(int)BufferEffectTriggerType.OnHit, "命中目标时"},
        };
        
        protected static readonly Dictionary<int, string> BufferEffectValidTypeDisplayStrings = new Dictionary<int, string>()
        {
            {(int)BufferEffectValidType.TimeInterval, "间隔时间"},
            {(int)BufferEffectValidType.Attribute, "属性"},
        };
        
        protected static readonly Dictionary<int, string> SourceAttributeTypeDisplayStrings = new Dictionary<int, string>()
        {
            {(int)AttributeType.Attack, "攻击力"},
            {(int)AttributeType.Defense, "防御力"},
            {(int)AttributeType.Hp, "生命值"},
        };
        
        protected static readonly Dictionary<int, string> AttributeTypeDisplayStrings = new Dictionary<int, string>()
        {
            {(int)AttributeType.Hp, "生命值"},
            {(int)AttributeType.Attack, "攻击力"},
            {(int)AttributeType.Defense, "防御力"},
            {(int)AttributeType.DamageBonus, "伤害加成"},
            {(int)AttributeType.DamageReduction, "伤害减免"},
            {(int)AttributeType.CriticalHitRate, "暴击率"},
            {(int)AttributeType.CriticalHitDamage, "暴击伤害"},
            {(int)AttributeType.MoveSpeed, "移动速度"},
        };
        
        protected static readonly Dictionary<int, string> AttackCategoryTypeDisplayStrings = new Dictionary<int, string>()
        {
            {(int)AttackCategoryType.Damage, "伤害"},
            {(int)AttackCategoryType.Heal, "恢复"},
        };
        
        protected static readonly Dictionary<int, string> BattleHitCategoryTypeDisplayStrings = new Dictionary<int, string>()
        {
            {(int)BattleHitCategory.Invalid, "无"},
            {(int)BattleHitCategory.Other, "敌方"},
            {(int)BattleHitCategory.SameWithoutMySelf, "除自己之外的己方"},
            {(int)BattleHitCategory.Myself, "自己"},
            {(int)BattleHitCategory.SameIncludingMySelf, "包括自己在内的己方"},
            {(int)BattleHitCategory.OtherAndSameWithoutMySelf, "除自己之外的己方和敌方"},
            {(int)BattleHitCategory.OtherAndSameIncludingMySelf, "己方和敌方"},
        };
        
        protected static readonly Dictionary<int, string> PassiveSkillEventTypeDisplayStrings = new Dictionary<int, string>()
        {
            {(int)PassiveSkillEventType.OnCreate, "当角色出生"},
            {(int)PassiveSkillEventType.OnDie, "当角色死亡"},
            {(int)PassiveSkillEventType.OnHit, "当命中目标"},
            {(int)PassiveSkillEventType.OnBeHit, "当被命中"},
            {(int)PassiveSkillEventType.OnChangeIn, "当替换进场"},
            {(int)PassiveSkillEventType.OnChangeOut, "当替换出场"},
            {(int)PassiveSkillEventType.OnKillEnemy, "当击杀敌人"},
            {(int)PassiveSkillEventType.OnHpChange, "当血量变化"},
            {(int)PassiveSkillEventType.OnAnimationEvent, "动画事件触发"},
        };
        
        protected static readonly Dictionary<int, string> PassiveSkillProConditionTypeDisplayStrings = new Dictionary<int, string>()
        {
            {(int)PassiveSkillProConditionType.TimeInterval, "间隔时间"},
            {(int)PassiveSkillProConditionType.Attribute, "属性"},
            {(int)PassiveSkillProConditionType.HasBuffer, "拥有buffer"},
        };
        
        protected static readonly Dictionary<int, string> SelectTargetTypeDisplayStrings = new Dictionary<int, string>()
        {
            {(int)SelectTargetType.Self, "自己"},
            {(int)SelectTargetType.Team, "队伍"},
        };
        
        protected static readonly Dictionary<int, string> SelectTargetFilterTypeDisplayStrings = new Dictionary<int, string>()
        {
            {(int)SelectTargetFilterType.Attribute, "属性"},
        };
        
        protected static readonly Dictionary<int, string> BufferOverlayTypeDisplayStrings = new Dictionary<int, string>()
        {
            {(int)BufferOverlayType.Invalid, "不可叠加，不可刷新"},
            {(int)BufferOverlayType.Overlay, "可叠加"},
            {(int)BufferOverlayType.Refresh, "可刷新"},
            {(int)BufferOverlayType.Cost, "可消耗"},
            {(int)BufferOverlayType.OverlayRefresh, "可叠加，可刷新"},
            {(int)BufferOverlayType.OverlayCost, "可叠加，可消耗"},
            {(int)BufferOverlayType.OverlayRefreshCost, "可叠加，可刷新,可消耗"},
        };
        
        protected static readonly Dictionary<int, string> BufferEndTypeDisplayStrings = new Dictionary<int, string>()
        {
            {(int)BufferEndType.AlwaysFulfilled, "永远满足(瞬间生效效果)"},
            {(int)BufferEndType.TimeOver, "时间(S)"},
            {(int)BufferEndType.Hit, "攻击命中"},
            {(int)BufferEndType.NoTargetBuffer, "没有目标buffer"},
        };
        
        protected static readonly Dictionary<int, string> ShellTypeDisplayStrings = new Dictionary<int, string>()
        {
            {(int)ShellType.Bullet, "子弹类"},
            {(int)ShellType.Area, "区域类"},
        };
        
        protected static readonly Dictionary<int, string> BulletTypeDisplayStrings = new Dictionary<int, string>()
        {
            {(int)BulletType.Normal, "普通"},
            {(int)BulletType.Penetrate, "穿透"},
        };
        
        protected static readonly Dictionary<int, string> ShakePowerTypeDisplayStrings = new Dictionary<int, string>()
        {
            {(int)BattleCameraShakeParam.ShakePower.NONE, "NONE"},
            {(int)BattleCameraShakeParam.ShakePower.XXS, "XXS"},
            {(int)BattleCameraShakeParam.ShakePower.XS, "XS"},
            {(int)BattleCameraShakeParam.ShakePower.S, "S"},
            {(int)BattleCameraShakeParam.ShakePower.M, "M"},
            {(int)BattleCameraShakeParam.ShakePower.L, "L"},
            {(int)BattleCameraShakeParam.ShakePower.XL, "XL"},
            {(int)BattleCameraShakeParam.ShakePower.XXL, "XXL"},
        };
        
        protected static readonly Dictionary<int, string> ShakeDirectionTypeDisplayStrings = new Dictionary<int, string>()
        {
            {(int)BattleCameraShakeParam.ShakeDirection.RANDOM, "随机"},
            {(int)BattleCameraShakeParam.ShakeDirection.VERTICAL, "水平"},
            {(int)BattleCameraShakeParam.ShakeDirection.HORIZON, "垂直"},
        };

        protected class NumericalDrawerData
        {
            public int Id;
            public int Level = 1;
            public IList<NumericalMessage> NumericalMessages;
            public DefinitionPbType PbType;
        }
        
        protected enum DefinitionPbType
        {
            PassiveSkill,
            Buffer,
            Attack,
            Shell,
        }

        protected static NumericalDrawerData NumericalData = new NumericalDrawerData();
        
        
        public override bool DrawMessage(IMessage message)
        {
            var descriptor = message.Descriptor;
            var isDirty = false;
            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                foreach (var field in descriptor.Fields.InDeclarationOrder())
                {
                    if (field.Name == "numericalValues")
                    {
                        NumericalData.Level = EditorGUILayout.IntSlider("数值等级(仅供查看)",NumericalData.Level,1,10);
                        NumericalData.NumericalMessages = (IList<NumericalMessage>)field.Accessor.GetValue(message);

                        if (message is PassiveSkillDefinitionMessage passiveSkill)
                        {
                            NumericalData.PbType = DefinitionPbType.PassiveSkill;
                            NumericalData.Id = passiveSkill.Id;
                        }
                        else if(message is BufferDefinitionMessage buffer)
                        {
                            NumericalData.PbType = DefinitionPbType.Buffer;
                            NumericalData.Id = buffer.Id;
                        }
                        else if(message is AttackDefinitionGroupMessage attack)
                        {
                            NumericalData.PbType = DefinitionPbType.Attack;
                            NumericalData.Id = attack.AttackGroupId;
                        }
                        else if(message is ShellDefinitionMessage shell)
                        {
                            NumericalData.PbType = DefinitionPbType.Shell;
                            NumericalData.Id = shell.Id;
                        }
                    }
                }
                
                foreach (var field in descriptor.Fields.InDeclarationOrder())
                {
                    isDirty |= DrawField(message, field);
                }

                foreach (var oneOf in descriptor.Oneofs)
                {
                    isDirty |= DrawOneOf(message, oneOf);
                }
            }

            return isDirty;
        }
        
        public override bool DrawOneOf(IMessage parent, OneofDescriptor descriptor)
        {
            var fieldNumbers = descriptor.Fields.Select(f => f.FieldNumber).ToArray();
            var displayNames = descriptor.Fields.Select(f => GetOneOfNameDisplay(descriptor.Name,f.Name)).ToArray();
            var currentField = descriptor.Accessor.GetCaseFieldDescriptor(parent);

            if (!PopupSelectIndices.ContainsKey(descriptor.Name))
            {
                var currentFieldNumber = currentField?.FieldNumber ?? 0;
                PopupSelectIndices.Add(descriptor.Name, currentFieldNumber);
            }

            using (var scope = new EditorGUI.ChangeCheckScope())
            {
                using (new GUILayout.VerticalScope(GUI.skin.box))
                {
                    var updated = EditorGUILayout.IntPopup(GetOneOfLabelDisplay(descriptor.Name), PopupSelectIndices[descriptor.Name],
                        displayNames, fieldNumbers);
                    if (!scope.changed)
                    {
                        return DrawField(parent, currentField, true);
                    }

                    PopupSelectIndices[descriptor.Name] = updated;
                    
                    var changedField = descriptor.Fields.FirstOrDefault(f => f.FieldNumber == updated);
                    if (changedField == null)
                    {
                        return DrawField(parent, currentField, true);
                    }
                    
                    if (changedField.FieldType == FieldType.Message)
                    {
                        changedField.Accessor.SetValue(parent,
                            Activator.CreateInstance(changedField.MessageType.ClrType));
                    }
                    else
                    {
                        changedField.Accessor.SetValue(parent, default);
                    }

                    DrawField(parent, changedField, true);
                }
            }
            
            return true;
        }

        protected virtual string GetOneOfNameDisplay(string descriptorName,string fieldName)
        {
            return fieldName;
        }
        
        protected virtual string GetOneOfLabelDisplay(string label)
        {
            return label;
        }
        
        protected bool OnNumericalValue(EditorGUI.ChangeCheckScope scope, IMessage parent, FieldDescriptor descriptor,string label)
        {
            if (NumericalData.NumericalMessages == null)
            {
                return false;
            }
            
            var index = (int)descriptor.Accessor.GetValue(parent);
            if (index >= NumericalData.NumericalMessages.Count)
            {
                descriptor.Accessor.SetValue(parent, 0);
                return false;
            }
            
            var displayStrings = new string[NumericalData.NumericalMessages.Count];
            for (int i = 0; i < NumericalData.NumericalMessages.Count; i++)
            {
                string displayString;
                if (NumericalData.NumericalMessages[i].Excel != 0)
                {
                    var excelIndex = NumericalData.NumericalMessages[i].Excel;
                    var excelValue = GetExcelValue(excelIndex);
                    displayString = $"[{i}]:表格配置arg_{excelIndex}：{excelValue}";
                }
                else
                {
                    displayString = $"[{i}]:固定值：{NumericalData.NumericalMessages[i].Local}";
                }

                displayStrings[i] = displayString;
            }
            
            var updated = EditorGUILayout.Popup(label, index, displayStrings);
            updated = updated < 0 ? 0 : updated;
            if (!scope.changed)
            {
                return false;
            }
            
            descriptor.Accessor.SetValue(parent, updated);
            
            return true;
        }

        private int GetExcelValue(int index)
        {
            if (NumericalData.PbType == DefinitionPbType.PassiveSkill)
            {
                // var numerical = LubanManager.Instance.Tables.TbBufferNumerical.Get(NumericalData.Id, 1);
                // return numerical.Args[index];
            }
            else if(NumericalData.PbType == DefinitionPbType.Buffer)
            {
                var numerical = LubanManager.GetEditorTables().TbBufferNumerical.Get(NumericalData.Id, NumericalData.Level);
                //Excel 从1开始 需要-1
                if (numerical != null && index <= numerical.Args.Length)
                {
                    return numerical.Args[index - 1];
                }
            }

            return 0;
        }
        
        protected bool OnDrawType(EditorGUI.ChangeCheckScope scope, IMessage parent, FieldDescriptor descriptor, Dictionary<int, string> dictionary,string label)
        {
            int value = (int)descriptor.Accessor.GetValue(parent);
            var types = dictionary.Keys.ToArray();
            var displayStrings         = dictionary.Values.ToArray();
            
            var updated = EditorGUILayout.Popup(label, types.IndexOf(value), displayStrings);
            updated = updated < 0 ? 0 : updated;
            if (!scope.changed)
            {
                return false;
            }
            descriptor.Accessor.SetValue(parent, types[updated]);
            return true;
        }
        
        protected bool DrawIntDisplayValue(EditorGUI.ChangeCheckScope scope, IMessage parent, FieldDescriptor descriptor,string label)
        {
            var value = (int)descriptor.Accessor.GetValue(parent);

            var updated = EditorGUILayout.IntField(label, value);
            updated = updated < 0 ? 0 : updated;
            if (!scope.changed)
            {
                return false;
            }
            
            descriptor.Accessor.SetValue(parent, updated);
            
            return true;
        }
        
        protected bool DrawBoolDisplayValue(EditorGUI.ChangeCheckScope scope, IMessage parent, FieldDescriptor descriptor,string label)
        {
            var value = (bool)descriptor.Accessor.GetValue(parent);
            
            var updated = EditorGUILayout.Toggle(label, value);
            if (!scope.changed)
            {
                return false;
            }

            descriptor.Accessor.SetValue(parent, updated);
            return true;
        }
    }
}