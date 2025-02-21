using System.Collections.Generic;
using UnityEngine;

namespace GameMain.Runtime
{
    public class AudioGroup
    {
        private readonly int _capacity;
        private float _volume = 1F;

        private List<AudioAgent> _agents = new List<AudioAgent>();

        public float Volume => _volume;

        public AudioGroup(int capacity)
        {
            _capacity = capacity;
        }

        public void Play(string name,bool is3D,Vector3 pos3D)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            AudioAgent candidateAgent = null;
            for (int i = 0; i < _agents.Count; i++)
            {
                if (_agents[i].Name == name)
                {
                    candidateAgent = _agents[i];
                    break;
                }
            }

            if (candidateAgent == null)
            {
                if (_agents.Count == _capacity)
                {
                    var oldAgent = GetStopAudioAgent();
                    oldAgent.SetName(name);
                    candidateAgent = oldAgent;
                }
                else
                {
                    var newAgent = new AudioAgent(this, name);
                    _agents.Add(newAgent);
                    candidateAgent = newAgent;
                }
            }

            candidateAgent.Play(is3D, pos3D);
        }
        public void SetVolume(float value)
        {
            _volume = value;
            foreach (var iAgent in _agents)
            {
                iAgent.ResetVolume();
            }
        }
        private AudioAgent GetStopAudioAgent()
        {
            AudioAgent agent = null;
            for (int i = 0; i < _agents.Count; i++)
            {
                if (_agents[i].IsAllStop())
                {
                    agent = _agents[i];
                    break;
                }
            }

            if (agent == null)
            {
                agent = _agents[0];
            }
                
            agent.Release();

            return agent;
        }
    }
}