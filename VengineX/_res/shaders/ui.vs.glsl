#version 450 core

uniform mat4 M;
uniform mat4 V;
uniform mat4 P;

layout(location = 0) in vec2 aPosition;
layout(location = 1) in vec2 aUV;		

out vec2 uv;

void main()
{
	uv = aUV;
	gl_Position = P * V * M * vec4(aPosition.xy, 0.0, 1.0);
}