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
    private int[] m_TrianglesX;
    private int[] m_TrianglesY;
    private int[] m_TrianglesZ;
	private Vector3[] m_Normals = null;

	



	/**************INPUT***************/
    [Header("Settings Generation")]

    public int m_SizeX, m_SizeY, m_SizeZ;
    public METHODE m_Methode = METHODE.SEPERATE_SIDES;

    public float m_MaxDiff = 1.5f;
    public float m_MinDiff = 0.0f;


    public bool m_Quantization = false;
    public float m_QuantizationSteps = 1.0f;
	
    [Header("Settings Random")]

    public int m_Seed = 200;


	public float m_NoiseScale = 2.0f;


	public float m_NoiseFactor = 0.05f;


    [Header("Settings Others")]

    [SerializeField]
    private bool m_GenerateMeshCollider = true;

    [Header("Mapping")]


    public Transform m_RootTrees;
    public GameObject m_TreePrefab;



    public ProceduralSphere m_CoverSphere;
    public bool m_RemoveCoveredTriangles = false;


    private MeshCollider m_MeshCollider;

	// Use this for initialization
	void Start () 
	{
		//GenerateSphere(true);

	}

	void OnValidate()
	{
		GenerateSphere(true);

    }


	
   


	public void GenerateSphere(bool _init = false)
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
				
                //can not be removed, since we need sperate uv coorinates
                //RemoveDoubles(); 
				
                //distribution is needed for making the verticies around the sphere at the end more even
				MakeDistribution();
			}
			
            //make every vertice have a distane of 1 from the origin
            MakeUniformSphere();
			
            //genereate Noise
            GenerateNoise();

			if(m_Quantization)
			{
				Quantization();
			}
		}


        if(m_RemoveCoveredTriangles && m_CoverSphere != null)
        {
            RemoveCoveredTriangels();

        }
		

        //assign mesh
        m_Mesh.vertices = m_Vertices;
        //m_Mesh.triangles = m_Triangles;

        m_Mesh.subMeshCount = 3;
        m_Mesh.SetTriangles(m_TrianglesZ, 0);
        m_Mesh.SetTriangles(m_TrianglesY, 1);
        m_Mesh.SetTriangles(m_TrianglesX, 2);

        m_Mesh.uv = m_UV;
  
        //recalculate normals -> Maybe we can do this?
        m_Mesh.RecalculateNormals();
		
        //avarages the normals on seem positions
        MakeNormalsSeemless ();
		
        //we need to reacalculate the tangens after assigning the normals
        m_Mesh.RecalculateTangents();
        
		m_Normals = m_Mesh.normals;

		GetComponent<MeshFilter>().mesh = m_Mesh;


        if(m_GenerateMeshCollider)
        {
            m_MeshCollider = this.gameObject.GetComponent<MeshCollider>();
            if(!m_MeshCollider)
            {
                m_MeshCollider = this.gameObject.AddComponent<MeshCollider>();
            }

            m_MeshCollider.sharedMesh = m_Mesh;

        }

    }

	void Quantization()
	{
		
		for (int i = 0; i < m_Vertices.Length; i++) 
		{
            m_Vertices[i] = QuantizateVector(m_Vertices[i]);
		}

	}

    Vector3 QuantizateVector(Vector3 _vec)
    {
        float mag = 0.0f;
        mag = _vec.magnitude;
        mag = mag - (mag % m_QuantizationSteps);
        return Vector3.ClampMagnitude(_vec, mag);
    }

    void RemoveCoveredTriangels()
    {
        List<int> m_Triangles = new List<int>();

        for(int i = 0; i < m_TrianglesX.Length;i+=3)
        {
            Vector3 one = m_Vertices[m_TrianglesX[i]];
            Vector3 two = m_Vertices[m_TrianglesX[i+1]];
            Vector3 three = m_Vertices[m_TrianglesX[i+2]];

            float d1 = m_CoverSphere.GetDistanceFromUniSphereCenter(one);
            float d2 = m_CoverSphere.GetDistanceFromUniSphereCenter(two);
            float d3 = m_CoverSphere.GetDistanceFromUniSphereCenter(three);
            float mag1 = one.magnitude*transform.localScale.x;
            float mag2 = two.magnitude* transform.localScale.x;
            float mag3 = three.magnitude* transform.localScale.x;
            if(d1 < mag1 || d2 < mag2 || d3 < mag3 )
            {
                m_Triangles.Add(m_TrianglesX[i]);
                m_Triangles.Add(m_TrianglesX[i+1]);
                m_Triangles.Add(m_TrianglesX[i+2]);
            }

        }

        m_TrianglesX = m_Triangles.ToArray();

        m_Triangles = new List<int>();

        for (int i = 0; i < m_TrianglesY.Length; i += 3)
        {
            Vector3 one = m_Vertices[m_TrianglesY[i]];
            Vector3 two = m_Vertices[m_TrianglesY[i + 1]];
            Vector3 three = m_Vertices[m_TrianglesY[i + 2]];

            float d1 = m_CoverSphere.GetDistanceFromUniSphereCenter(one);
            float d2 = m_CoverSphere.GetDistanceFromUniSphereCenter(two);
            float d3 = m_CoverSphere.GetDistanceFromUniSphereCenter(three);
            float mag1 = one.magnitude* transform.localScale.x;
            float mag2 = two.magnitude* transform.localScale.x;
            float mag3 = three.magnitude* transform.localScale.x;
            if (d1 < mag1 || d2 < mag2 || d3 < mag3)
            {
                m_Triangles.Add(m_TrianglesY[i]);
                m_Triangles.Add(m_TrianglesY[i + 1]);
                m_Triangles.Add(m_TrianglesY[i + 2]);
            }

        }

        m_TrianglesY = m_Triangles.ToArray();

        m_Triangles = new List<int>();

        for (int i = 0; i < m_TrianglesZ.Length; i += 3)
        {
            Vector3 one = m_Vertices[m_TrianglesZ[i]];
            Vector3 two = m_Vertices[m_TrianglesZ[i + 1]];
            Vector3 three = m_Vertices[m_TrianglesZ[i + 2]];

            float d1 = m_CoverSphere.GetDistanceFromUniSphereCenter(one);
            float d2 = m_CoverSphere.GetDistanceFromUniSphereCenter(two);
            float d3 = m_CoverSphere.GetDistanceFromUniSphereCenter(three);
            float mag1 = one.magnitude* transform.localScale.x;
            float mag2 = two.magnitude* transform.localScale.x;
            float mag3 = three.magnitude* transform.localScale.x;
            if (d1 < mag1 || d2 < mag2 || d3 < mag3)
            {
                m_Triangles.Add(m_TrianglesZ[i]);
                m_Triangles.Add(m_TrianglesZ[i + 1]);
                m_Triangles.Add(m_TrianglesZ[i + 2]);
            }

        }

        m_TrianglesZ = m_Triangles.ToArray();

    }
    //returns absolut
    public float GetDistanceFromUniSphereCenter(Vector3 _point)
    {
        _point.Normalize();
        _point.x = _point.x/2.0f;
        _point.y = _point.y/2.0f;
        _point.z = _point.z/2.0f;


        //todo needs to take quantization into account
        return (_point + GetOffsetForPosition(_point)).magnitude* transform.localScale.x;

    }

    Vector3 GetOffsetForPosition(Vector3 square)
    {
        //dont do this, init it only once!
        GradientNoise gnoise = new GradientNoise(m_Seed);
        //noise scale is for how fine graded the noise is.
        //we need a combination between fine and rough noise
        float noiseF = gnoise.GetValue(square.y * m_NoiseScale,square.x*m_NoiseScale,square.z*m_NoiseScale);
    
        //normal of the vertex
        Vector3 normal =  square - this.transform.position;
        normal.Normalize();
        float nFactor =  noiseF * m_NoiseFactor; 
        return normal *nFactor;
    }

    private Ray m_CastRay = new Ray();
    public Vector3 ProjectPointOnToPlanet(Vector3 _point ,out Vector3 _normal, bool _presize = true)
    {
        Vector3 result = Vector3.zero;
        _normal = Vector3.one;
        _point.Normalize();


        if(!_presize || !m_MeshCollider)
        {
            _normal = _point;

            return GetDistanceFromUniSphereCenter(_point) *_point * this.transform.localScale.x;
        }
        else
        {
            m_CastRay.direction = -_point;
            m_CastRay.origin = (_point * this.transform.localScale.x);


            Debug.DrawRay(m_CastRay.origin,m_CastRay.direction*this.transform.localScale.x);

            RaycastHit hitInfo = new RaycastHit();
            m_MeshCollider.Raycast(m_CastRay, out hitInfo, this.transform.localScale.x);


            _normal = hitInfo.normal;
            result = hitInfo.point;
        }


        return result;

       // collider
    }


	void GenerateNoise()
	{
		//Persistence
		//PinkNoise noise = new PinkNoise(m_Seed);//new GradientNoise(200);
		
        //seems to be more smooth then pink noise

		float scale = m_NoiseScale;
        float factor = m_NoiseFactor;///this.transform.localScale.x;
		Vector3 offset = new Vector3(0.5f,0.5f,0.5f);
		for (int i = 0; i < m_Vertices.Length; i++) 
		{
			Vector3 square = m_Vertices[i];//+offset;

            m_Vertices[i] = m_Vertices[i] + GetOffsetForPosition(square);



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
			
            //go through all verticies, if you find verticies which are on the same position, avarage normal
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


        //uncomment again
        for (int i = 0; i < m_TrianglesX.Length;i++)
		{
			m_TrianglesX[i] = mapping[m_TrianglesX[i]];
		}
        for (int i = 0; i < m_TrianglesY.Length; i++)
        {
            m_TrianglesY[i] = mapping[m_TrianglesY[i]];
        }
        for (int i = 0; i < m_TrianglesZ.Length; i++)
        {
            m_TrianglesZ[i] = mapping[m_TrianglesZ[i]];
        }

	}




	private void CreateTriangles () {
		int quads = ((m_SizeX-1) * (m_SizeZ-1)*2) + ((m_SizeX-1) * (m_SizeY-1))*2 + ((m_SizeZ-1) * (m_SizeY-1))*2;//(m_SizeX * m_SizeY + m_SizeX * m_SizeZ + m_SizeY * m_SizeZ) * 2;

        m_TrianglesX = new int[quads * 2];
        m_TrianglesY = new int[quads * 2];
        m_TrianglesZ = new int[quads * 2];

		
		int index = 0;
		int offset = 0;
		


        //Y
        for(int x = 0; x < m_SizeX-1; x++ )
		{
			for(int z = 0; z < m_SizeZ-1; z++ )
			{
                index = SetQuad(m_TrianglesY,index,x*m_SizeX + z, x*m_SizeX +  z+1,(x+1)*m_SizeX +z  , (x+1)*m_SizeX +z+1);
			}
		}
      
		offset = (m_SizeX) * (m_SizeZ);

        for (int x = 0; x < m_SizeX - 1; x++)
        {
            for (int z = 0; z < m_SizeZ - 1; z++)
            {
                index = SetQuad(m_TrianglesY,index, offset+ x*m_SizeX +  z+1, offset+  x*m_SizeX + z, offset+   (x+1)*m_SizeX +z+1, offset+ (x+1)*m_SizeX +z);
			}
		}
     
        offset += (m_SizeX) * (m_SizeZ);
        //offset = 0;
        index = 0;
        //Z
		for(int x = 0; x < m_SizeX-1; x++ )
		{
			for(int y = 0; y < m_SizeY-1; y++ )
			{
                index = SetQuad(m_TrianglesZ,index, offset+ x*m_SizeX +  y+1,  offset+ x*m_SizeX + y,  offset+ (x+1)*m_SizeX +y+1, offset+ (x+1)*m_SizeX +y  );
			}

		}


		offset += (m_SizeX) * (m_SizeY);
		
		for(int x = 0; x < m_SizeX-1; x++ )
		{
			for(int y = 0; y < m_SizeY-1; y++ )
			{
                index = SetQuad(m_TrianglesZ,index, offset+ x*m_SizeX + y, offset+ x*m_SizeX +  y+1, offset+ (x+1)*m_SizeX +y  , offset+ (x+1)*m_SizeX +y+1);
			}

		}
		offset += (m_SizeX) * (m_SizeY);
        //offset = 0;
        index = 0;

		for(int z = 0; z < m_SizeZ-1; z++ )
		{
			for(int y = 0; y < m_SizeY-1; y++ )
			{
                index = SetQuad(m_TrianglesX,index, offset+ z*m_SizeX + y, offset+ z*m_SizeX +  y+1, offset+ (z+1)*m_SizeX +y  , offset+ (z+1)*m_SizeX +y+1);
			}
		}

		offset += (m_SizeZ) * (m_SizeY);
     
		for(int z = 0; z < m_SizeZ-1; z++ )
		{
			for(int y = 0; y < m_SizeY-1; y++ )
			{
                index = SetQuad(m_TrianglesX,index, offset+ z*m_SizeX +  y+1, offset+ z*m_SizeX + y, offset+ (z+1)*m_SizeX +y+1, offset+ (z+1)*m_SizeX +y   );
			}

		}
		
	}

	private static int SetQuad (int[] triangles, int i, int v00, int v10, int v01, int v11) 
    {
        
		//Debug.Log(i);
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
