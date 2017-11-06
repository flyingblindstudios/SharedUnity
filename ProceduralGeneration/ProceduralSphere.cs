using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoherentNoise.Generation;
using CoherentNoise.Generation.Fractal;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralSphere : MonoBehaviour {
/*   U 
	LFRB
	 D
	*/
	
	
	enum DISTRIBUTION {LINEAR};

	public enum METHODE {SEPERATE_SIDES};

	enum FACE {UP, LEFT, FRONT,RIGHT, BACK, DOWN}
	private Mesh m_Mesh;
	private Vector3[] m_Vertices;
    private Vector2[] m_UV;
    private int[] m_Triangles;
	private Vector3[] m_Normals = null;

	public int m_SizeX, m_SizeY, m_SizeZ;
	public METHODE m_Methode = METHODE.SEPERATE_SIDES;



	/**************INPUT***************/
	public int m_Seed = 200;
	private int m_Seed_private = 200;

	public float m_NoiseScale = 2.0f;
	private float m_NoiseScale_private = 2.0f;

	public float m_NoiseFactor = 0.05f;
	private float m_NoiseFactor_private = 0.05f;

	public float m_MaxDiff = 1.5f;
	public float m_MinDiff = 0.0f;


	// Use this for initialization
	void Awake () 
	{
		m_Seed_private = m_Seed;
		m_NoiseScale_private = m_NoiseScale;
		m_NoiseFactor_private = m_NoiseFactor;
		
		GenerateSphere(true);
		
	}

	void Update()
	{
		#if UNITY_EDITOR
			InEditorUpdate();
		#endif

	}
	
	void InEditorUpdate()
	{	
		if(m_Seed_private != m_Seed)
		{
			m_Seed_private = m_Seed;
			GenerateSphere();
			//
		} 

		if(m_NoiseScale_private != m_NoiseScale)
		{
			m_NoiseScale_private = m_NoiseScale;
			GenerateSphere();
			//
		}


		if(m_NoiseFactor_private != m_NoiseFactor)
		{
			m_NoiseFactor_private = m_NoiseFactor;
			GenerateSphere();
			//
		}

	}

	void GenerateSphere(bool _init = false)
	{
		
		if(m_SizeY < 2)
		{
			return;
		}
		
		m_Mesh = new Mesh();
		m_Mesh.name = "Procedural Cube";

		if(_init)
		{
            int size = CalculateNumberOfVerticies(m_SizeX, m_SizeY, m_SizeZ);
            m_Vertices = new Vector3[size];
            m_UV = new Vector2[size];

        }


		if(METHODE.SEPERATE_SIDES ==  m_Methode)
		{
			if(_init)
			{
				GenerateWithSeperateSides();
				CreateTriangles ();
				//RemoveDoubles(); 
				
				MakeDistribution();
			}
			MakeUniformSphere();
			GenerateNoise();
		}
		m_Mesh.vertices = m_Vertices;
		m_Mesh.triangles = m_Triangles;
        m_Mesh.uv = m_UV;
        m_Mesh.RecalculateNormals();
		//MakeNormalsSeemless ();
		m_Mesh.RecalculateTangents();
        
		m_Normals = m_Mesh.normals;
        //todo!!!:: set the normals of the edges to the same !
        //GetComponent<MeshRenderer>()
		GetComponent<MeshFilter>().mesh = m_Mesh;
    }


	void GenerateNoise()
	{
		//Persistence
		PinkNoise noise = new PinkNoise(m_Seed_private);//new GradientNoise(200);
		GradientNoise gnoise = new GradientNoise(m_Seed_private);
		float scale = m_NoiseScale_private;
		float factor = m_NoiseFactor_private/this.transform.localScale.x;
		Vector3 offset = new Vector3(0.5f,0.5f,0.5f);
		for (int i = 0; i < m_Vertices.Length; i++) 
		{
			Vector3 square = m_Vertices[i];//+offset;
			float noiseF = noise.GetValue(square.x*scale,square.y*scale,square.z*scale);
		

			Vector3 normal =  square - this.transform.position;
			normal.Normalize();

			//Debug.Log(noiseF);
			float nFactor =  noiseF * factor; //Mathf.Clamp(noiseF * factor,m_MinDiff,m_MaxDiff);

			

			m_Vertices[i] = m_Vertices[i] + normal *nFactor;


			float mag = (m_Vertices[i] - this.transform.position).magnitude;

			//if(mag > 0.1f)
			{
			

				//m_Vertices[i] = m_Vertices[i] + normal * gnoise.GetValue(square.x*scale*1,square.y,square.z*scale*1);// * factor;

			}

		}


	}

	void GenerateWithSeperateSides()
	{
		
		
		
		
		//generate x, z sides
		int index = 0;

		float offset = 0.5f;

		
		//down
		for(int x = 0; x < m_SizeX; x++ )
		{
			for(int z = 0; z < m_SizeZ; z++ )
			{
				m_Vertices[index] = new Vector3(((float)x/(float)(m_SizeX-1))-offset,-offset, ((float)z/(float)(m_SizeZ-1))-offset);

                float correctionX = 1.0f;// Mathf.Sqrt(1f - m_Vertices[index].y * m_Vertices[index].y * 0.5f - m_Vertices[index].z * m_Vertices[index].z * 0.5f - m_Vertices[index].y * m_Vertices[index].y * m_Vertices[index].z * m_Vertices[index].z * 1.0f / 3.0f);
                float correctionZ = 1.0f;// Mathf.Sqrt(1f - m_Vertices[index].y * m_Vertices[index].y * 0.5f - m_Vertices[index].x * m_Vertices[index].x * 0.5f - m_Vertices[index].y * m_Vertices[index].y * m_Vertices[index].x * m_Vertices[index].x * 1.0f / 3.0f); 
                m_UV[index] = new Vector2((float)x* correctionX / (float)(m_SizeX - 1) , (float)z * correctionZ / (float)(m_SizeZ - 1));
               
                index++;
			}

		}

		//up
		for(int x = 0; x < m_SizeX; x++ )
		{
			for(int z = 0; z < m_SizeZ; z++ )
			{
				m_Vertices[index] = new Vector3(((float)x/(float)(m_SizeX-1))-offset,offset, ((float)z/(float)(m_SizeZ-1))-offset);
                float correctionX = 1.0f;// Mathf.Sqrt(1f - m_Vertices[index].y * m_Vertices[index].y * 0.5f - m_Vertices[index].z * m_Vertices[index].z * 0.5f - m_Vertices[index].y * m_Vertices[index].y * m_Vertices[index].z * m_Vertices[index].z * 1.0f / 3.0f);
                float correctionZ = 1.0f;// Mathf.Sqrt(1f - m_Vertices[index].y * m_Vertices[index].y * 0.5f - m_Vertices[index].x * m_Vertices[index].x * 0.5f - m_Vertices[index].y * m_Vertices[index].y * m_Vertices[index].x * m_Vertices[index].x * 1.0f / 3.0f);

                m_UV[index] = new Vector2((float)x * correctionX / (float)(m_SizeX - 1), (float)z * correctionZ / (float)(m_SizeZ - 1));
                index++;
			}

		}

		//Back
		for(int x = 0; x < m_SizeX; x++ )
		{
			for(int y = 0; y < m_SizeY; y++ )
			{
				m_Vertices[index] = new Vector3(((float)x/(float)(m_SizeX-1))-offset,((float)y/(float)(m_SizeY-1))-offset, -offset);

                float correctionX = 1.0f;// Mathf.Sqrt(1f - m_Vertices[index].y * m_Vertices[index].y * 0.5f - m_Vertices[index].z * m_Vertices[index].z * 0.5f - m_Vertices[index].y * m_Vertices[index].y * m_Vertices[index].z * m_Vertices[index].z * 1.0f / 3.0f);
                float correctionY = 1.0f;// Mathf.Sqrt(1f - m_Vertices[index].x * m_Vertices[index].x * 0.5f - m_Vertices[index].z * m_Vertices[index].z * 0.5f - m_Vertices[index].x * m_Vertices[index].x * m_Vertices[index].z * m_Vertices[index].z * 1.0f / 3.0f);

                m_UV[index] = new Vector2((float)x* correctionX / (float)(m_SizeX - 1), (float)y* correctionY / (float)(m_SizeY - 1));
               
                index++;
			}

		}


		//front
		for(int x = 0; x < m_SizeX; x++ )
		{
			for(int y = 0; y < m_SizeY; y++ )
			{
				m_Vertices[index] = new Vector3(((float)x/(float)(m_SizeX-1))-offset,((float)y/(float)(m_SizeY-1))-offset, offset);

                float correctionX = 1.0f;//= Mathf.Sqrt(1f - m_Vertices[index].y * m_Vertices[index].y * 0.5f - m_Vertices[index].z * m_Vertices[index].z * 0.5f - m_Vertices[index].y * m_Vertices[index].y * m_Vertices[index].z * m_Vertices[index].z * 1.0f / 3.0f);
                float correctionY = 1.0f;// Mathf.Sqrt(1f - m_Vertices[index].x * m_Vertices[index].x * 0.5f - m_Vertices[index].z * m_Vertices[index].z * 0.5f - m_Vertices[index].x * m_Vertices[index].x * m_Vertices[index].z * m_Vertices[index].z * 1.0f / 3.0f);


                m_UV[index] = new Vector2((float)x* correctionX / (float)(m_SizeX - 1), (float)y* correctionY / (float)(m_SizeY - 1));
                index++;
			}

		}
		

		//left
		for(int z = 0; z < m_SizeZ; z++ )
		{
			for(int y = 0; y < m_SizeY; y++ )
			{

                float correctionZ = 1.0f;// Mathf.Sqrt(1f - m_Vertices[index].y * m_Vertices[index].y * 0.5f - m_Vertices[index].x * m_Vertices[index].x * 0.5f - m_Vertices[index].y * m_Vertices[index].y * m_Vertices[index].x * m_Vertices[index].x * 1.0f / 3.0f);
                float correctionY = 1.0f;// Mathf.Sqrt(1f - m_Vertices[index].x * m_Vertices[index].x * 0.5f - m_Vertices[index].z * m_Vertices[index].z * 0.5f - m_Vertices[index].x * m_Vertices[index].x * m_Vertices[index].z * m_Vertices[index].z * 1.0f / 3.0f);


                m_Vertices[index] = new Vector3(-offset, ((float)y / (float)(m_SizeY - 1)) - offset, ((float)z/(float)(m_SizeZ-1))-offset);
                m_UV[index] = new Vector2(  (float)y * correctionY / (float)(m_SizeY - 1),(float)z* correctionZ / (float)(m_SizeZ - 1));

                index++;
			}

		}
		
		//right
		for(int z = 0; z < m_SizeZ; z++ )
		{
			for(int y = 0; y < m_SizeY; y++ )
			{

                float correctionZ = 1.0f;// Mathf.Sqrt(1f - m_Vertices[index].y * m_Vertices[index].y * 0.5f - m_Vertices[index].x * m_Vertices[index].x * 0.5f - m_Vertices[index].y * m_Vertices[index].y * m_Vertices[index].x * m_Vertices[index].x * 1.0f / 3.0f);
                float correctionY = 1.0f;// Mathf.Sqrt(1f - m_Vertices[index].x * m_Vertices[index].x * 0.5f - m_Vertices[index].z * m_Vertices[index].z * 0.5f - m_Vertices[index].x * m_Vertices[index].x * m_Vertices[index].z * m_Vertices[index].z * 1.0f / 3.0f);


                m_Vertices[index] = new Vector3(offset,((float)y/(float)(m_SizeY-1))-offset, ((float)z/(float)(m_SizeZ-1))-offset);
                m_UV[index] = new Vector2((float)z * correctionZ / (float)(m_SizeZ - 1), (float)y * correctionY / (float)(m_SizeY - 1));
                index++;
			}

		}
		


	}

	void MakeNormalsSeemless()
	{
		Vector3[] normals = m_Mesh.normals;
		List<int> avg = new List<int>();
		for(int i = 0; i < m_Vertices.Length;i++)
		{
			avg.Clear();
			Vector3 newNormal = normals[i];
			for(int k = 0; k < m_Vertices.Length; k++)
			{
				//if(/*(m_Vertices[i] - m_Vertices[k]).sqrMagnitude < 0.00001f*/)
				if(m_Vertices[i].x == m_Vertices[k].x && m_Vertices[i].y == m_Vertices[k].y && m_Vertices[i].z == m_Vertices[k].z)
				{
					avg.Add(k);
				}
			}

			if(avg.Count > 1)
			{
				
				newNormal =  Vector3.zero;
				for(int k = 0; k < avg.Count;k++) 
				{
					newNormal+=m_Vertices[avg[k]];
				}
				newNormal /= (float)avg.Count;

				for(int k = 0; k < avg.Count; k++)
				{
						normals[avg[k]] = newNormal.normalized;	
				}
			
			}
			else
			{
				Debug.Log("found only one");
			}

			
		}
		m_Mesh.normals = normals;
		
		
	}

	private void RemoveDoubles()
	{
		List<Vector3> newVerticies = new List<Vector3>();
        List<Vector2> newUV = new List<Vector2>();
        int[] mapping = new int[  m_Vertices.Length ];

		for(int i = 0; i < m_Vertices.Length;i++)
		{
			bool contains = false;
			int mappingValue = newVerticies.Count;
			for(int k = 0; k < newVerticies.Count;k++)
			{
				if((m_Vertices[i] - newVerticies[k]).sqrMagnitude < 0.00001f)
				{
					contains = true;
					//Debug.Log("found equal " + m_Vertices[i] + " " + newVerticies[k]);
					mappingValue = k;
				}
				

			}
			
			
			if(!contains)
			{
				newVerticies.Add(m_Vertices[i]);
                newUV.Add(m_UV[i]);
            }

			mapping[i] = mappingValue;	
			
		}

		m_Vertices = newVerticies.ToArray();
        m_UV = newUV.ToArray();

        //m_Triangles

        for (int i = 0; i < m_Triangles.Length;i++)
		{
			m_Triangles[i] = mapping[m_Triangles[i]];
		}

	}




	private void CreateTriangles () {
		int quads = ((m_SizeX-1) * (m_SizeZ-1)*2) + ((m_SizeX-1) * (m_SizeY-1))*2 + ((m_SizeZ-1) * (m_SizeY-1))*2;//(m_SizeX * m_SizeY + m_SizeX * m_SizeZ + m_SizeY * m_SizeZ) * 2;
		m_Triangles = new int[quads * 6];
		
		int index = 0;
		int offset = 0;
		for(int x = 0; x < m_SizeX-1; x++ )
		{
			for(int z = 0; z < m_SizeZ-1; z++ )
			{
				index = SetQuad(m_Triangles,index,x*m_SizeX + z, x*m_SizeX +  z+1,(x+1)*m_SizeX +z  , (x+1)*m_SizeX +z+1);
			}
		}

		offset = (m_SizeX) * (m_SizeZ);
		
		for(int x = 0; x < m_SizeX-1; x++ )
		{
			for(int z = 0; z < m_SizeZ-1; z++ )
			{

				index = SetQuad(m_Triangles,index, offset+ x*m_SizeX +  z+1, offset+  x*m_SizeX + z, offset+   (x+1)*m_SizeX +z+1, offset+ (x+1)*m_SizeX +z);
			}
		}
		offset += (m_SizeX) * (m_SizeZ);
	
		for(int x = 0; x < m_SizeX-1; x++ )
		{
			for(int y = 0; y < m_SizeY-1; y++ )
			{
				index = SetQuad(m_Triangles,index, offset+ x*m_SizeX +  y+1,  offset+ x*m_SizeX + y,  offset+ (x+1)*m_SizeX +y+1, offset+ (x+1)*m_SizeX +y  );
			}

		}
		offset += (m_SizeX) * (m_SizeY);
		
		for(int x = 0; x < m_SizeX-1; x++ )
		{
			for(int y = 0; y < m_SizeY-1; y++ )
			{
				index = SetQuad(m_Triangles,index, offset+ x*m_SizeX + y, offset+ x*m_SizeX +  y+1, offset+ (x+1)*m_SizeX +y  , offset+ (x+1)*m_SizeX +y+1);
			}

		}
		offset += (m_SizeX) * (m_SizeY);

		for(int z = 0; z < m_SizeZ-1; z++ )
		{
			for(int y = 0; y < m_SizeY-1; y++ )
			{
				index = SetQuad(m_Triangles,index, offset+ z*m_SizeX + y, offset+ z*m_SizeX +  y+1, offset+ (z+1)*m_SizeX +y  , offset+ (z+1)*m_SizeX +y+1);
			}
		}

		offset += (m_SizeZ) * (m_SizeY);

		for(int z = 0; z < m_SizeZ-1; z++ )
		{
			for(int y = 0; y < m_SizeY-1; y++ )
			{
				index = SetQuad(m_Triangles,index, offset+ z*m_SizeX +  y+1, offset+ z*m_SizeX + y, offset+ (z+1)*m_SizeX +y+1, offset+ (z+1)*m_SizeX +y   );
			}

		}
		
	}

	private static int SetQuad (int[] triangles, int i, int v00, int v10, int v01, int v11) {
//		Debug.Log(i);
		triangles[i] = v00;
		triangles[i + 1] = triangles[i + 4] = v01;
		triangles[i + 2] = triangles[i + 3] = v10;
		triangles[i + 5] = v11;
		return i + 6;
	}

	void MakeDistribution()
	{
		
		//float maxDistX = 0.5f;
		
		for (int i = 0; i < m_Vertices.Length; i++) 
		{
			Vector3 square = m_Vertices[i];
			
			//change distribution, so that the resulting sphere is more uniform!
			m_Vertices[i].x = square.x * Mathf.Sqrt(1f - square.y * square.y * 0.5f - square.z * square.z * 0.5f - square.y * square.y * square.z * square.z * 1.0f/3.0f );
			m_Vertices[i].y = square.y * Mathf.Sqrt(1f - square.x * square.x * 0.5f - square.z * square.z * 0.5f - square.x * square.x * square.z * square.z * 1.0f/3.0f  );
			m_Vertices[i].z = square.z * Mathf.Sqrt(1f - square.y * square.y * 0.5f - square.x * square.x * 0.5f - square.y * square.y * square.x * square.x * 1.0f/3.0f  );
		
			//m_Vertices[i].z = m_Vertices[i].y * Mathf.Sqrt(1f - m_Vertices[i].x * m_Vertices[i].x * 0.5f);

			// float oneDistance = 1.0f/m_SizeX;

		//	float factorx =  (Mathf.Abs(m_Vertices[i].x))/maxDistX;
			//float factory =  (Mathf.Abs(m_Vertices[i].y))/maxDistX;
			//float factorz =  (Mathf.Abs(m_Vertices[i].z))/maxDistX;

			//muse linear distance one step reletaive to maxdistance with function?
			//factorx = Mathf.Exp(factorx-1.0f);
			//factory = Mathf.Exp(factory-1.0f);
			//factorz = Mathf.Exp(factorz-1.0f);

		//	factorx = Mathf.Pow( factorx,0.5f);
//factory = Mathf.Pow( factory,0.5f);
			//factorz = Mathf.Pow( factorz,0.5f);
//
			

			/*factorx = Mathf.Pow( 0.5f,factorx);
			factory = Mathf.Pow( 0.5f,factory);
			factorz = Mathf.Pow( 0.5f,factorz);*/

			//m_Vertices[i].x = m_Vertices[i].x * factorx;
			//m_Vertices[i].y = m_Vertices[i].y * factory;
			//m_Vertices[i].z = m_Vertices[i].z * factorz;
		}
	}

	void MakeUniformSphere()
	{
		
		for (int i = 0; i < m_Vertices.Length; i++) {

			m_Vertices[i].Normalize();
			m_Vertices[i].x = m_Vertices[i].x/2.0f;
			m_Vertices[i].y = m_Vertices[i].y/2.0f;
			m_Vertices[i].z = m_Vertices[i].z/2.0f;

		}
	}



	private void OnDrawGizmos () {
		
		
		return;
		if (m_Vertices == null) {
			return;
		}
		Gizmos.color = Color.white;
		for (int i = 0; i < m_Vertices.Length; i++) {

			if(m_Vertices[i] == null)
			{
				break;

			}

			Gizmos.DrawSphere(m_Vertices[i], 0.01f);
		}

		if (m_Normals == null) {
			return;
		}
		for (int i = 0; i < m_Normals.Length; i++) {

			if(m_Normals[i] == null)
			{
				break;

			}

			Gizmos.DrawLine(m_Vertices[i], m_Vertices[i]+m_Normals[i] );
		}


		
	}

	//_y needs to be at least 2
	int CalculateNumberOfVerticies(int _x, int _y, int _z)
	{
		//2*(x*y) + ((z-2)*(x*y)-((x-2)*(y-2)))

		if(m_Methode == METHODE.SEPERATE_SIDES)
		{
			return (2*_x*_z) + (2*_x*_y) + (2*_z*_y);
		}


		return 2*(_x*_z) + ((_y-2)*(_x*_z)-((_x-2)*(_z-2)));
	}

	int GetIndex (int _x, int _y, FACE _face)
	{
		return 0;
	}

}
