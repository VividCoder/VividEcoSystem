#version 330 core

in vec2 UV;


out vec4 color;

uniform vec3 viewPos;
uniform vec3 lightPos;
uniform vec3 lightCol;
uniform float lightRange;
uniform vec3 lightSpec;

// Values that stay constant for the whole mesh.
uniform sampler2D posMap;
uniform sampler2D normMap;
uniform sampler2D normsMap;
uniform sampler2D colMap;
uniform sampler2D tanMap;

void main(){
 
  vec3 fPos = texture(posMap,UV).rgb;
  vec3 fNorms = texture(normsMap,UV).rgb;
  vec3 fNorm = texture(normMap,UV).rgb;
  vec3 fCol = texture(colMap,UV).rgb;
  vec3 fTan = texture(tanMap,UV).rgb;


  vec3 T = texture(tanMap,UV).rgb;

  vec3 N = fNorms;

  vec3 worldPos = fPos;

  T = normalize(T-dot(T,N)*N);

  vec3 B = cross(N,T);

  mat3 TBN = transpose(mat3(T,B,N));

  vec3 TLP = TBN * lightPos;

  vec3 TVP = TBN * viewPos;


  vec3 TFP = TBN  * fPos;

 float dis = abs(length(fPos-lightPos));

 float dv = dis/lightRange;

 if(dv>1.0) dv = 1.0;

 dv = 1.0-dv;

 vec3 normal = fNorm;

 normal = normalize(normal * 2.0 - 1.0);



 vec3 col = fCol;

// vec3 ambient = vec30.1;


 vec3 lightDir = normalize(TLP - TFP);

 float diff = max(dot(lightDir,normal),0.0);

 vec3 diffuse = vec3(diff,diff,diff);

 vec3 viewDir = normalize(TVP-TFP);

 vec3 reflectDir = reflect(-lightDir,normal);
 vec3 halfwayDir = normalize(lightDir+viewDir);

 float spec = pow(max(dot(normal,halfwayDir),0.0),32.0);

 vec3 specular = (lightSpec) * spec;

  diffuse = diffuse * dv;

    vec3 final = diffuse + specular;

    final = final;

    final = final * fCol;

    final = vec3(spec,spec,spec);

    final = final;

    final = final + (fCol * diffuse);

    final = final * dv;

    color = vec4(final,1.0);


return;
 

  




 
}
