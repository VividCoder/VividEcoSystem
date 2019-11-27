#version 330 core

// Interpolated values from the vertex shaders


in vec3 fNorm;

// Ouput data
out vec3 colorout;



// Values that stay constant for the whole mesh.
uniform sampler2D colMap;

void main(){
 
  
colorout = fNorm;

return;
 

  




 
}
