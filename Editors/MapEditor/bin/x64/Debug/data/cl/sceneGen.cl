
__kernel void genScene(__global const float* vert,__global float* vout)
{

    int id = get_global_id(0);

    vout[id] = vert[id];

}