#version 330 core

layout(location = 0) out vec3 oPos;
layout(location = 1) out vec3 oNorm;
layout(location = 2) out vec3 oNorms;
layout(location = 3) out vec3 oCol; 
layout(location = 4) out vec3 oTan;

// Interpolated values from the vertex shaders

in vec2 texCoords;
in vec3 fPos;
in vec3 fNorm;
in vec3 fTan;




// Values that stay constant for the whole mesh.
uniform sampler2D colMap;
uniform sampler2D normMap;


void main(){
 
oPos = fPos;
oNorm = texture(normMap,texCoords).rgb;
oNorms = fNorm;
oCol = texture(colMap,texCoords).rgb;
oTan = fTan;


return;
 

  




 
}
