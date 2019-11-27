#version 330 core

// Interpolated values from the vertex shaders

in vec2 texCoords;
in vec3 fPos;

// Ouput data
out vec3 colorout;

uniform bool texCol;

// Values that stay constant for the whole mesh.
uniform sampler2D colMap;

void main(){
 
  
colorout = fPos;

return;
 

  




 
}
