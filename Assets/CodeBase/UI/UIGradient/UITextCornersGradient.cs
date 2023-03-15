using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CodeBase.UI.UIGradient
{
	[AddComponentMenu("UI/Effects/Text 4 Corners Gradient")]
	public class UITextCornersGradient : BaseMeshEffect {
		[FormerlySerializedAs("m_topLeftColor")] [SerializeField] private Color topLeftColor = Color.white;
		[FormerlySerializedAs("m_topRightColor")] [SerializeField] private Color topRightColor = Color.white;
		[FormerlySerializedAs("m_bottomRightColor")] [SerializeField] private Color bottomRightColor = Color.white;
		[FormerlySerializedAs("m_bottomLeftColor")] [SerializeField] private Color bottomLeftColor = Color.white;

		public override void ModifyMesh(VertexHelper _vh)
		{
			if(enabled)
			{
				Rect rect = graphic.rectTransform.rect;

				UIVertex vertex = default(UIVertex);
				for (int i = 0; i < _vh.currentVertCount; i++) {
					_vh.PopulateUIVertex (ref vertex, i);
					Vector2 normalizedPosition = UIGradientUtils.VerticePositions[i % 4];
					vertex.color *= UIGradientUtils.Bilerp(bottomLeftColor, bottomRightColor, topLeftColor, topRightColor, normalizedPosition);
					_vh.SetUIVertex (vertex, i);
				}
			}
		}
	}
}
