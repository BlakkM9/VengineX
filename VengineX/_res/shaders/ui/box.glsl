#version 450 core

uniform vec4 uColor;

in vec2 uv;
layout(location = 0) out vec4 fColor;

void main()
{
	fColor = uColor;
}