// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Simplified Diffuse shader. Differences from regular Diffuse one:
// - only Main Color
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "Mobile/Diffuse-OnlyColor" {
Properties {
	_Color("Main Color", Color) = (1,1,1,1)
}
SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 150

CGPROGRAM
#pragma surface surf Lambert noforwardadd

fixed4 _Color;

struct Input {
	float4 color : COLOR;
};

void surf (Input IN, inout SurfaceOutput o) {
    o.Albedo = _Color.rgb;
    o.Alpha = _Color.a;
}
ENDCG
}

Fallback "Mobile/VertexLit"
}
