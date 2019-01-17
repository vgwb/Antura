struct SurfaceOutputSpecularAntura
{
	fixed3 Albedo;  // diffuse color
	fixed3 Normal;  // tangent space normal, if written
	fixed3 Emission;
	fixed3 SpecularColor;
	half Specular;  // specular power in 0..1 range
	fixed Gloss;    // specular intensity
	fixed Alpha;    // alpha for transparencies
};

inline fixed4 LightingAnturaBlinnPhong(SurfaceOutputSpecularAntura s, fixed3 lightDir, fixed3 halfDir, fixed atten)
{
	fixed diff = max(0, dot(s.Normal, lightDir));
	fixed nh = max(0, dot(s.Normal, halfDir));
	fixed spec = pow(nh, s.Specular * 128) * s.Gloss;

	fixed4 c;
	c.rgb = (s.Albedo * _LightColor0.rgb * diff + s.SpecularColor * _LightColor0.rgb * spec) * atten;
	c.a = s.Alpha;
	return c;
}

inline fixed4 LightingAnturaGlass(SurfaceOutputSpecularAntura s, fixed3 lightDir, fixed3 halfDir, fixed atten)
{
	fixed diff = max(0, dot(s.Normal, lightDir));
	fixed nh = max(0, dot(s.Normal, halfDir));
	fixed spec = pow(nh, s.Specular * 128) * s.Gloss;

	fixed4 c;
	c.rgb = (s.Albedo * _LightColor0.rgb * diff + s.SpecularColor * _LightColor0.rgb * spec) * atten;
	c.a = s.Alpha + spec;
	return c;
}