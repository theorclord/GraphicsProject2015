using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSimulation
{
    class CameraController
    {
        public double[] Position { get; set; }
        double[] Facing { get; set; }

        public CameraController(double[] position)
        {
            Position = position;
            Facing = new double[3];
        }

        public OpenTK.Matrix4 Update(double timeSinceLastFrame)
        {
            OpenTK.Matrix4 transMatrix;
            OpenTK.Matrix4 rotMatrix; //, facingMatrix;
            double[] rotation = new double[3];
            double[] translation = new double[3];

            KeyboardUpdate(timeSinceLastFrame, ref translation, ref rotation);
            rotMatrix = CreateRotationMatrix(rotation);
            //facingMatrix = CreateRotationMatrix(Facing);

            transMatrix = OpenTK.Matrix4.CreateTranslation(new OpenTK.Vector3((float)translation[0], (float)translation[1], (float)translation[2]));
            transMatrix = OpenTK.Matrix4.Mult(transMatrix, rotMatrix);
            //transMatrix = OpenTK.Matrix4.Mult(transMatrix, facingMatrix);

            for (int i = 0; i < 3; i++)
            { Facing[i] = (Facing[i] + rotation[i]) % 360; }

            return transMatrix;
        }

        private void KeyboardUpdate(double timeSinceLastFrame, ref double[] refTranslation, ref double[] refRotation)
        {
            OpenTK.Input.KeyboardState keyState = OpenTK.Input.Keyboard.GetState();
            
            /*
             * By convention, the double arrays are ordered in x,y,z order (x = 0, y = 1, z = 2).
             * Orientation is right handed, with an inverted Z-axis.
             * 
             * Translation is converted during calculation to account for facing.
             */
            if (keyState.IsKeyDown(OpenTK.Input.Key.W))
            { refTranslation[2] += 500000 * Math.Cos(Facing[1]); }

            if (keyState.IsKeyDown(OpenTK.Input.Key.S))
            { refTranslation[2] += -500000 * Math.Cos(Facing[1]); }

            if (keyState.IsKeyDown(OpenTK.Input.Key.D))
            { refTranslation[0] += 500000 * Math.Cos(Facing[1]); }

            if (keyState.IsKeyDown(OpenTK.Input.Key.A))
            { refTranslation[0] += -500000 * Math.Cos(Facing[1]); }

            if (keyState.IsKeyDown(OpenTK.Input.Key.Q))
            { refTranslation[1] += 500000 * Math.Cos(Facing[2]); }

            if (keyState.IsKeyDown(OpenTK.Input.Key.E))
            { refTranslation[1] += -500000 * Math.Cos(Facing[2]); }

            if (keyState.IsKeyDown(OpenTK.Input.Key.Keypad7))
            { refRotation[2] += 1; }
            
            if (keyState.IsKeyDown(OpenTK.Input.Key.Keypad9))
            { refRotation[2] += -1; }

            if (keyState.IsKeyDown(OpenTK.Input.Key.Keypad8))
            { refRotation[0] += 1; }

            if (keyState.IsKeyDown(OpenTK.Input.Key.Keypad5))
            { refRotation[0] += -1; }

            if (keyState.IsKeyDown(OpenTK.Input.Key.Keypad4))
            { refRotation[1] += 1; }

            if (keyState.IsKeyDown(OpenTK.Input.Key.Keypad6))
            { refRotation[1] += -1; }

            if (keyState.IsKeyDown(OpenTK.Input.Key.ShiftLeft) || keyState.IsKeyDown(OpenTK.Input.Key.ShiftRight))
            {
                for (int i = 0; i < refTranslation.Length; i++)
                { refTranslation[i] *= 10; }
            }

            if (keyState.IsKeyDown(OpenTK.Input.Key.ControlLeft))
            {
                for (int i = 0; i < refTranslation.Length; i++)
                { refTranslation[i] *= 10; }
            }

            for (int i = 0; i < 3; i++)
            {
                refRotation[i] *= timeSinceLastFrame;
                refTranslation[i] *= timeSinceLastFrame;
                Position[i] += refTranslation[i];
            }
        }

        private OpenTK.Matrix4 CreateRotationMatrix(double[] rotationAngles)
        {
            OpenTK.Matrix4 rotationMatrix = OpenTK.Matrix4.Identity;
            var rotXMat = OpenTK.Matrix4.CreateFromAxisAngle(new OpenTK.Vector3(1.0f, 0, 0), (float)rotationAngles[0]);
            var rotYMat = OpenTK.Matrix4.CreateFromAxisAngle(new OpenTK.Vector3(0, 1.0f, 0), (float)rotationAngles[1]);
            var rotZMat = OpenTK.Matrix4.CreateFromAxisAngle(new OpenTK.Vector3(0, 0, 1.0f), (float)rotationAngles[2]);

            rotationMatrix = OpenTK.Matrix4.Mult(rotationMatrix, rotXMat);
            rotationMatrix = OpenTK.Matrix4.Mult(rotationMatrix, rotYMat);
            rotationMatrix = OpenTK.Matrix4.Mult(rotationMatrix, rotZMat);

            return rotationMatrix;
        }
    }
}
