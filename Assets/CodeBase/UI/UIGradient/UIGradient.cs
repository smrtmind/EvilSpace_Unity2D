using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CodeBase.UI.UIGradient 
{
	[AddComponentMenu("UI/Effects/Gradient")]
	public class UIGradient : BaseMeshEffect {
		[FormerlySerializedAs("m_color1")] [SerializeField] private Color color1 = Color.white;
		[FormerlySerializedAs("m_color2")] [SerializeField] private Color color2 = Color.white;
		[FormerlySerializedAs("m_angle")] [SerializeField][Range(-180f, 180f)] private float angle = 0f;
		[FormerlySerializedAs("m_ignoreRatio")] [SerializeField]private bool ignoreRatio = true;

		public override void ModifyMesh(VertexHelper _vh) {
			if (enabled) {
				Rect rect = graphic.rectTransform.rect;
				Vector2 dir = UIGradientUtils.RotationDir(angle);

				if (!ignoreRatio)
					dir = UIGradientUtils.CompensateAspectRatio(rect, dir);

				UIGradientUtils.Matrix2X3 localPositionMatrix = UIGradientUtils.LocalPositionMatrix(rect, dir);

				UIVertex vertex = default(UIVertex);
				for (int i = 0; i < _vh.currentVertCount; i++) {
					_vh.PopulateUIVertex(ref vertex, i);
					Vector2 localPosition = localPositionMatrix * vertex.position;
					vertex.color *= Color.Lerp(color2, color1, localPosition.y);
					_vh.SetUIVertex(vertex, i);
				}
			}
		}
	}
}
