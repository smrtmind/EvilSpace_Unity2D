using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CodeBase.UI.UIGradient
{
	[AddComponentMenu("UI/Effects/Text Gradient")]
	public class UITextGradient : BaseMeshEffect
	{
		[FormerlySerializedAs("m_color1")] [SerializeField] private Color color1 = Color.white;
		[FormerlySerializedAs("m_color2")] [SerializeField] private Color color2 = Color.white;
		[FormerlySerializedAs("m_angle")] [SerializeField] [Range(-180f, 180f)] private float angle = 0f;

		public override void ModifyMesh(VertexHelper _vh)
		{
			if(enabled)
			{
				Rect rect = graphic.rectTransform.rect;
				Vector2 dir = UIGradientUtils.RotationDir(angle);
				UIGradientUtils.Matrix2X3 localPositionMatrix = UIGradientUtils.LocalPositionMatrix(new Rect(0f, 0f, 1f, 1f), dir);

				UIVertex vertex = default(UIVertex);
				for (int i = 0; i < _vh.currentVertCount; i++) {

					_vh.PopulateUIVertex (ref vertex, i);
					Vector2 position = UIGradientUtils.VerticePositions[i % 4];
					Vector2 localPosition = localPositionMatrix * position;
					vertex.color *= Color.Lerp(color2, color1, localPosition.y);
					_vh.SetUIVertex (vertex, i);
				}
			}
		}
	}
}
