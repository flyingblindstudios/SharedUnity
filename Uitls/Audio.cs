using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Audio : MonoBehaviour 
{
	List<AudioSource> m_AudioSources = null;

	void Awake()
	{
		m_AudioSources = new List<AudioSource>();
	}
	public AudioSource PlayAudio(AudioClip _clip, bool _loop = false)
	{
		int freeAudio = -1;

		for(int i = 0; i < m_AudioSources.Count; i++)
		{
			if(!m_AudioSources[i].isPlaying)
			{
				freeAudio = i;
				break;
			}
		}

		if(freeAudio == -1)
		{
			m_AudioSources.Add(this.gameObject.AddComponent<AudioSource>());
			freeAudio = m_AudioSources.Count -1;
		}
		
		
		m_AudioSources[freeAudio].Stop();
		m_AudioSources[freeAudio].clip = _clip;
		m_AudioSources[freeAudio].loop = _loop;
		m_AudioSources[freeAudio].Play();


		return m_AudioSources[freeAudio];
	}



	
}
