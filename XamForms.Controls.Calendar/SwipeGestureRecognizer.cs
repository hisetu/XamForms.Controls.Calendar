using System;
using Xamarin.Forms;

namespace XamForms.Controls
{
    public delegate void EventMoveHandler(object sender, float movement);

    public class SwipeGestureRecognizer : PanGestureRecognizer
    {
        private int _distanceRequired;
        public int DistanceRequired
        {
            get => _distanceRequired;
        }
        private int _startX;
        private int _distance;
        private bool _ignoreTouch;

        public event EventHandler StartMovement;
        public event EventHandler SwipeLeft;
        public event EventHandler SwipeRight;
        public event EventHandler DismissSwipe;
        public event EventMoveHandler Movement;

        public SwipeGestureRecognizer(int distanceRequired = 150)
        {
            _distanceRequired = distanceRequired;
            PanUpdated += SwipeGestureRecognizerPanUpdated;
        }

        public void SwipeGestureRecognizerPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    HandleTouchStart();
                    break;
                case GestureStatus.Running:
                    HandleTouch((float)e.TotalX);
                    break;
                case GestureStatus.Completed:
                    HandleTouchEnd();
                    break;
            }
        }

        private void HandleTouchStart()
        {
            _startX = 0;
            StartMovement?.Invoke(this, new EventArgs());
        }

        private void HandleTouch(float diffX)
        {
            if (_ignoreTouch == false)
            {
                _distance = (int)diffX;

                Movement?.Invoke(this, diffX);
            }
        }

        private void HandleTouchEnd()
        {
            _ignoreTouch = true;

            if (Math.Abs((int)_distance) > _distanceRequired)
            {
                if (_distance > 0)
                {
                    SwipeRight?.Invoke(this, new EventArgs());
                }
                else
                {
                    SwipeLeft?.Invoke(this, new EventArgs());
                }
            }
            else
            {
                DismissSwipe?.Invoke(this, new EventArgs());
            }

            _ignoreTouch = false;
        }
    }
}
