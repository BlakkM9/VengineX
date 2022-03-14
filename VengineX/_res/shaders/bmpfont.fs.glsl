﻿#version 450 core

uniform sampler2D uTexture;
uniform vec4 uColor;

in vec2 uv;
layout(location = 0) out vec4 fColor;

void main()
{
	vec4 sampled = vec4(1.0, 1.0, 1.0, texture(uTexture, vec2(uv.x, 1 - uv.y)).r);
	fColor = clamp((uColor * sampled) + vec4(0.2), 0, 1);
//	fColor = vec4(uv, 0, 1);
	fColor = sampled;
//	fColor = vec4(1);
}