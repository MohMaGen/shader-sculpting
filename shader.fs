#version 330

in vec2 fragTexCoord;
in vec4 fragColor;
out vec4 finalColor;

uniform vec4  fogColor;
uniform float fogIntensity;
uniform float time;

uniform vec4 colors[10];


float gtz(float v) { return v >= -0.005 ? 1. : -1.; }

mat3 roty(float a)
{
    return mat3(
         cos(a), 0, sin(a),
         0,      1, 0,
        -sin(a), 0, cos(a)
    );
}

mat4 look_at_matrix(vec3 pos, vec3 target, vec3 up)
{
    vec3 zaxis = normalize(pos - target);
    vec3 xaxis = normalize(cross(up, zaxis));
    vec3 yaxis = cross(zaxis, xaxis);

    return mat4(
        xaxis.x, yaxis.x, zaxis.x, pos.x,
        xaxis.y, yaxis.y, zaxis.y, pos.y,
        xaxis.z, yaxis.z, zaxis.z, pos.z,
        0, 0, 0, 1
    );
}

float fogFactor(float t) {
    return clamp(1 / exp(t * t * fogIntensity), 0, 1);
}


float soft_max(float d1, float d2, float k ) {
    float h = clamp( 0.5 - 0.5*(d2+d1)/k, 0.0, 1.0 );
    return mix( d2, -d1, h ) + k*h*(1.0-h);
}

float soft_min( float d1, float d2, float k ) {
    float h = clamp( 0.5 + 0.5*(d2-d1)/k, 0.0, 1.0 );
    return mix( d2, d1, h ) - k*h*(1.0-h);
}
vec3 soft_minv(vec3 n1, vec3 n2, float k) {
    float h = clamp( 0.5 + 0.5*dot(n1,n2)/k, 0.0, 1.0 );
    return mix(n2, n1, h) - k*h*(1.0-h);
}

vec3 rot_y(vec3 v, float a)
{
    return v * mat3(cos(a), 0, sin(a), 0, 1, 0, -sin(a), 0, cos(a));
}

vec3 proj(vec3 a, vec3 n)
{
    return dot(a, n) * n;
}

vec3 projpl(vec3 a, vec3 n)
{
	return a + proj(a, n);
}


float sdfPlane(vec3 pos, vec3 n, float r)
{
    vec3 p = pos*n;
    return abs(p.x + p.y + p.z + r) / length(n) - 0.01;
}

float sdfSphere(vec3 pos, vec3 center, float radius)
{
    return length(pos - center) - radius;
}

float sdfCube(vec3 pos, vec3 center, float a)
{
    vec3 d = abs(pos - center) - a;
    return min(max(d.x, max(d.y, d.z)), 0) + length(max(d,0));
}

float sdfScene(vec3 pos)
{
    float dist = 0;

    dist = sdfSphere(pos, vec3(0), 0.5);
    dist = max(dist, sdfCube(pos, vec3(0), 0.35));

    dist = soft_min(dist, sdfPlane(pos, vec3(1), 0), 0.2);
    dist = max(dist, -sdfSphere(pos, vec3(0), 0.40));
    dist = max(dist, -sdfCube(pos, vec3(-0.2), 0.2));
    dist = max(dist, sdfCube(pos, vec3(0), 0.6));

    return max(0, dist);
}

float raymatch(vec3 ray_origin, vec3 ray_direction)
{
    float ret = 0;
    float dist; vec3 pos;
    for (int i = 0; i < 100; ++i) {
        pos   = ray_origin + ray_direction * ret;
        dist  = sdfScene(pos);
        ret  += dist;
    }

    return ret;
}

vec3 normalCube(vec3 pos, vec3 center, float a)
{
    return normalize((pos - center) * gtz(sdfCube(pos, center, a)));
}

vec3 normalSphere(vec3 pos, vec3 center, float radius)
{
    return normalize((pos - center) * gtz(sdfSphere(pos, center, radius)));
}


vec3 normalPlane(vec3 pos, vec3 n, float r)
{
    return normalize(pos * n + r) * gtz(sdfPlane(pos, n, r));
}


vec3 normalScene(vec3 pos)
{
    vec3 normal = normalSphere(pos, vec3(0), 0.50);
    float dist = sdfSphere(pos, vec3(0), 0.50);

    if (sdfCube(pos, vec3(0), 0.35) >= dist)         normal = normalCube(pos, vec3(0), 0.35);
    dist = max(dist, sdfCube(pos, vec3(0), 0.35));

	if (sdfPlane(pos, vec3(1), 0) <= dist)  normal = soft_minv(normalPlane(pos, vec3(1), 0), normal, 0.2);
    dist = soft_min(dist, sdfPlane(pos, vec3(1), 0), 0.2);

    if (-sdfSphere(pos, vec3(0), 0.40) >= dist)      normal = normalSphere(pos, vec3(0), 0.40);
    dist = max(dist, -sdfSphere(pos, vec3(0), 0.40));

    if (-sdfCube(pos, vec3(-0.2), 0.2) >= dist)      normal = normalCube(pos, vec3(-0.2), 0.2);
    dist = max(dist, -sdfCube(pos, vec3(-0.2), 0.2));

    if (sdfCube(pos, vec3(0), 0.6) >= dist)          normal = normalCube(pos, vec3(0), 0.6);
    dist = max(dist, sdfCube(pos, vec3(0), 0.6));

    return normalize(normal);
}



void main() {
    vec3 light_direction = normalize(vec3(-1, 1, 1));
    light_direction     *= roty(-time*2);
    float big_distance   = 10;

    vec2 uv = fragTexCoord - 0.5;
    vec3 camera_px = vec3(uv.xy, - 1);

    vec3 look_at  = vec3(0);
    vec3 view_pos = vec3(2, -2, 0);
    view_pos     *= roty(time);
    vec3 forward  = normalize(look_at-view_pos);

    mat4 look_at_matrix = look_at_matrix(view_pos, look_at, vec3(0,1,0));

    vec3 ray_origin    = (vec4(camera_px, 1) * look_at_matrix).xyz;
    vec3 ray_direction = ray_origin - view_pos;

    float dist = raymatch(ray_origin, ray_direction);
    vec3  pos  = ray_origin + ray_direction * dist;

    finalColor = vec4(0);
    finalColor = colors[int(round(length(pos) * 20)) % 6+3];

    vec3  light_start = pos - light_direction * big_distance;
    float light_dist  = raymatch(light_start, light_direction);
    vec3  light_pos   = light_start + light_direction * light_dist;


	/*
    if (dot(look_at - pos, normalScene(pos)) > 0) {
        finalColor = colors[3];
    } else {
        finalColor = colors[4];
    }
    */

    float light_intense = dot(normalScene(pos), light_direction);
    vec4 light_color  = clamp(colors[5] + vec4(0.3, 0.3, 0.3, 1.), 0, 1);
    vec4 shadow_color = clamp(colors[7] - vec4(0.2, 0.2, 0.2, 1.), 0, 1);
    if (length(light_pos - pos) < 0.01) {
        finalColor = mix(finalColor, light_color, light_intense * 0.7);
    } else {
        finalColor = mix(finalColor, shadow_color, 0.4);
    }

    float fog_factor = fogFactor(length(ray_origin - pos));
    finalColor       = mix(fogColor, finalColor, fog_factor);
}
