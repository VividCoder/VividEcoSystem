#version 330 core

// Input vertex data, different for all executions of this shader.
layout(location = 0) in vec3 aPos;
layout(location = 1) in vec2 aTexCoords;
layout(location = 2) in vec3 aNormal;
layout(location = 3) in vec3 aBiTangent;
layout(location = 4) in vec3 aTangent;


// Values that stay constant for the whole mesh.
uniform mat4 view;
uniform mat4 model;
uniform mat4 proj;

out vec2 texCoords;
out vec3 fPos;
out vec3 fNorm;
out vec3 fBi;
out vec3 fTan;

void main(){

    texCoords = aTexCoords;

    
    vec4 ff = model * vec4(aPos,1.0);

    fPos = ff.xyz;

    mat3 normalMatrix = transpose(inverse(mat3(model)));

	fNorm = normalize(normalMatrix * aNormal);

    fTan = normalize(normalMatrix * aTangent);

    

	gl_Position = proj * view * model* vec4(aPos,1.0);

	
}

