using System.Collections.Generic;
using System.Linq;
using Better.EditorTools.EditorAddons.Helpers;
using UnityEditor;

namespace Better.Validation.EditorAddons.WindowModule.CollectionDrawing
{
    public class DefaultDrawer : CollectionDrawer
    {
        private List<ValidationCommandData> _dataList;
        
        public override int Order { get; } = 0;

        public override string GetOptionName()
        {
            return "Default";
        }

        public override CollectionDrawer Initialize(List<ValidationCommandData> data)
        {
            _dataList = data;
            return this;
        }

        public override int Count => _dataList.Count;

        public override void DrawCollection()
        {
            foreach (var data in _dataList)
            {
                DrawBox(data);
                EditorGUILayout.Space(DrawersHelper.SpaceHeight);
            }
        }

        public override void ClearResolved()
        {
            _dataList.RemoveAll(x =>
            {
                x.Revalidate();
                return x.IsValid;
            });
        }

        public override ValidationCommandData GetNext()
        {
            return GetAtDirection(1);
        }
        
        public override ValidationCommandData GetPrevious()
        {
            return GetAtDirection(-1);
        }

        private ValidationCommandData GetAtDirection(int direction)
        {
            if (_currentItem == null)
            {
                _currentItem = _dataList.First();
            }
            else
            {
                var index = _dataList.IndexOf(_currentItem);
                index += direction;
                if (index >= _dataList.Count)
                {
                    index = 0;
                }

                _currentItem = _dataList[index];
            }

            return _currentItem;
        }
        
        public override void Revalidate()
        {
            foreach (var commandData in _dataList)
            {
                commandData.Revalidate();
            }
        }

        public override bool IsValid()
        {
            return _dataList != null;
        }

        protected override List<ValidationCommandData> GetRemaining()
        {
            return _dataList;
        }
    }
}