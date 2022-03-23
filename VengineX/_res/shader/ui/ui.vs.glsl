#version 460

uniform mat4 V;
uniform mat4 P;


layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec4 aColor;
layout (location = 2) in vec2 aUV;


out VertexOut
{
	vec4 color;
	vec2 uv;
} o;


void main()
{
	o.color = aColor;
	o.uv = aUV;
	gl_Position = P * V * vec4(aPosition, 1.0);
}