#version 330 core

// Input vertex data, different for all executions of this shader.
layout(location = 0) in vec3 vP;
layout(location = 1) in vec2 vUV;
layout(location = 2) in vec4 vCol;


// Values that stay constant for the whole mesh.
uniform mat4 view;
uniform mat4 model;
uniform mat4 proj;

out vec2 UV;

void main(){

    UV = vUV;

	gl_Position = proj * vec4(vP,1);

	
}

