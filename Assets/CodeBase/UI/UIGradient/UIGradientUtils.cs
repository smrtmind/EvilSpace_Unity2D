using UnityEngine;

namespace CodeBase.UI.UIGradient
{
	public static class UIGradientUtils
	{
		public struct Matrix2X3
		{
			public float m00, m01, m02, m10, m11, m12;
			public Matrix2X3(float _m00, float _m01, float _m02, float _m10, float _m11, float _m12)
			{
				this.m00 = _m00;
				this.m01 = _m01;
				this.m02 = _m02;
				this.m10 = _m10;
				this.m11 = _m11;
				this.m12 = _m12;
			}

			public static Vector2 operator*(Matrix2X3 _m, Vector2 _v)
			{
				float x = (_m.m00 * _v.x) - (_m.m01 * _v.y) + _m.m02;
				float y = (_m.m10 * _v.x) + (_m.m11 * _v.y) + _m.m12;
				return new Vector2(x, y);
			}
		}

		public static Matrix2X3 LocalPositionMatrix(Rect _rect, Vector2 _dir)
		{
			float cos = _dir.x;
			float sin = _dir.y;
			Vector2 rectMin = _rect.min;
			Vector2 rectSize = _rect.size;
			float c = 0.5f;
			float ax = rectMin.x / rectSize.x + c;
			float ay = rectMin.y / rectSize.y + c;
			float m00 = cos / rectSize.x;
			float m01 = sin / rectSize.y;
			float m02 = -(ax * cos - ay * sin - c);
			float m10 = sin / rectSize.x;
			float m11 = cos / rectSize.y;		
			float m12 = -(ax * sin + ay * cos - c);
			return new Matrix2X3(m00, m01, m02, m10, m11, m12);
		}

		public static Vector2[] VerticePositions { get; } = new Vector2[] { Vector2.up, Vector2.one, Vector2.right, Vector2.zero };

		public static Vector2 RotationDir(float _angle)
		{
			float angleRad = _angle * Mathf.Deg2Rad;
			float cos = Mathf.Cos(angleRad);
			float sin = Mathf.Sin(angleRad);
			return new Vector2(cos, sin);
		}

		public static Vector2 CompensateAspectRatio(Rect _rect, Vector2 _dir)
		{
			float ratio = _rect.height / _rect.width;
			_dir.x *= ratio;
			return _dir.normalized;
		}

		public static float InverseLerp (float _a, float _b, float _v)
		{
			return _a != _b ? (_v - _a) / (_b - _a) : 0f;
		}

		public static Color Bilerp(Color _a1, Color _a2, Color _b1, Color _b2, Vector2 _t)
		{
			Color a = Color.LerpUnclamped(_a1, _a2, _t.x);
			Color b = Color.LerpUnclamped(_b1, _b2, _t.x);
			return Color.LerpUnclamped(a, b, _t.y);
		}

		public static void Lerp(UIVertex _a, UIVertex _b, float _t, ref UIVertex _c)
		{
			_c.position = Vector3.LerpUnclamped(_a.position, _b.position, _t);
			_c.normal = Vector3.LerpUnclamped(_a.normal, _b.normal, _t);
			_c.color = Color32.LerpUnclamped(_a.color, _b.color, _t);
			_c.tangent = Vector3.LerpUnclamped(_a.tangent, _b.tangent, _t);
			_c.uv0 = Vector3.LerpUnclamped(_a.uv0, _b.uv0, _t);
			_c.uv1 = Vector3.LerpUnclamped(_a.uv1, _b.uv1, _t);
		}
	}
}
