using System;
using DotNetBrowser.Geometry;
using DotNetBrowser.Input.Mouse;
using DotNetBrowser.Input.Mouse.Events;
using UnityEngine;

namespace Assets.Scripts
{
    internal class MouseHelper
    {
        private readonly IMouse mouse;
        private readonly bool[] pressStatus = {false, false, false, false, false};

        public float ScrollSpeed = 1000.0f;

        public MouseHelper(IMouse mouse)
        {
            this.mouse = mouse;
        }

        public Size ViewSize { get; set; }

        public Point Point { get; set; }

        private void OnMousePressed(Point location, MouseButton button)
        {
            if (pressStatus[(int) button]) return;

            pressStatus[(int) button] = true;
            MousePressedEventArgs args = new MousePressedEventArgs
            {
                Location = location,
                Button = button,
                ClickCount = 1
            };
            mouse.Pressed.Raise(args);
        }

        private void OnMouseDragged(Point location, MouseButton button)
        {
            MouseDraggedEventArgs args = new MouseDraggedEventArgs
            {
                Location = location,
                Button = button
            };
            mouse.Dragged.Raise(args);
        }

        private void OnMouseReleased(Point location, MouseButton button)
        {
            if (!pressStatus[(int) button]) return;

            pressStatus[(int) button] = false;
            MouseReleasedEventArgs args = new MouseReleasedEventArgs
            {
                Location = location,
                Button = button,
                ClickCount = 1
            };
            mouse.Released.Raise(args);
        }

        public void MouseWheel()
        {
            Point p = Point ?? GetMousePoint();
            if (p != null)
            {
                MouseWheelMovedEventArgs args = new MouseWheelMovedEventArgs
                {
                    Location = p,
                    DeltaY = Input.GetAxis("Mouse ScrollWheel") * ScrollSpeed
                };
                mouse.WheelMoved.Raise(args);
            }
        }

        public void MouseDown()
        {
            MouseEvent(Input.GetMouseButtonDown, OnMousePressed);
        }

        public void MouseDrag()
        {
            MouseEvent(Input.GetMouseButton, OnMouseDragged);
        }

        public void MouseUp()
        {
            MouseEvent(Input.GetMouseButtonUp, OnMouseReleased);
        }

        public void MouseMoved()
        {
            Point point3D = GetMousePoint();
            Point p = Point ?? point3D;

            if (p != null)
            {
                MouseMovedEventArgs args = new MouseMovedEventArgs
                {
                    Location = p
                };
                mouse.Moved.Raise(args);
            }
        }

        private Point GetMousePoint()
        {
            if (Camera.main != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    int x = (int)(ViewSize.Width * hit.textureCoord.x);
                    int y = (int)(ViewSize.Height * hit.textureCoord.y);
                    return new Point(x, y);
                }
            }

            return null;
        }

        private void MouseEvent(Func<int, bool> mouseButtonUsed, Action<Point, MouseButton> mouseAction)
        {
            Point p = Point ?? GetMousePoint();
            Point = null;

            if (p != null)
            {
                if (mouseButtonUsed(0)) mouseAction(p, MouseButton.Left);
                if (mouseButtonUsed(1)) mouseAction(p, MouseButton.Right);
                if (mouseButtonUsed(2)) mouseAction(p, MouseButton.Middle);
            }
        }
    }
}