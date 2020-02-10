using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Python
{
    // A class that spawn a grid like platform, with an edge texture
    public class Grid
    {
        private GraphicsDevice gd;

        // The side length of the grid
        public int Length;

        // Variables for the grid models
        private List<GridModule> _gridPositions = new List<GridModule>();
        private Model _model;

        // Variables for the edge models
        private List<CustomModel> _edgeModules = new List<CustomModel>();
        private Model _edgeModel;

        // Variables that determine the distance between gridModules
        private float _modelCenterDistance;
        private float _modelScale;

        

        // Constructor
        public Grid(int length, float modelScale, float modelGap, ContentManager content, GraphicsDevice graphicsDevice)
        {
            // Check if it's a odd number, otherwise there won't be a middle point and throw
            int numberCheck = length % 2;
            if (numberCheck != 1) throw new ArgumentOutOfRangeException();
            else
            {
                Length = length;
            }
            // Load the default themed models
            Load(content, 0);
            _modelScale = modelScale;
            _modelCenterDistance = modelGap;
            gd = graphicsDevice;
            // Generate grid and edge
            Generate(Length);
            GenerateEdge(Length);
        }

        // Load all the models for the grid, able to change theme 
        public void Load(ContentManager content, int theme)
        {
            _model = content.Load<Model>(@"Models/GridModel_" + theme);
            _edgeModel = content.Load<Model>(@"Models/GridEdgeModel_" + theme);
        }
        

        // Method for updating the size/models
        public void Generate(int length)
        {
            // Check if it's a odd number, otherwise there won't be a middle point and throw
            int numberCheck = length % 2;
            if (numberCheck != 1) throw new ArgumentOutOfRangeException();
            else
            {
                // Calculate the distance between 0 and the edge of the grid
                int edgeDistance = EdgeDistanceFromOrigin(Length);
                // Clear the already existing gridPositions
                _gridPositions.Clear();
                // For every possible y coordinate, generate every possible x coordinate and for every x coordinate, create a GridModule
                for (int y = -edgeDistance; y <= edgeDistance; y++)
                for (int x = -edgeDistance; x <= edgeDistance; x++)
                {
                    // Create a module based on variables
                    _gridPositions.Add(new GridModule(x, y, _modelCenterDistance, _modelScale, _model, gd));
                }
            }
        }

        // A method for generating an edge on the map
        public void GenerateEdge(int length)
        {
            // Check if it's a correct number between min and max, otherwise there won't be a middle point and throw exception
            int numberCheck = length % 2;
            if (numberCheck != 1) throw new ArgumentOutOfRangeException();
            else
            {
                // Calculate the distance between 0 and the edge of the grid
                int edgeDistance = EdgeDistanceFromOrigin(Length);
                // Clear the already existing edgePositions
                _edgeModules.Clear();
                // For every length (edge minus to plus) create a customModel at each gridPoint and rotate it towards center
                for (int i = -edgeDistance; i <= edgeDistance; i++)
                {
                    // This is west edge
                    _edgeModules.Add(new CustomModel(_edgeModel, Position(-edgeDistance - 1, i) - new Vector3(-_modelCenterDistance * _modelScale / 2, -_modelCenterDistance * _modelScale / 2, 0), new Vector3(3*MathHelper.PiOver2, 3*MathHelper.PiOver2, 0), new Vector3(_modelScale), gd));
                    // This is east edge
                    _edgeModules.Add(new CustomModel(_edgeModel, Position(edgeDistance, i) -      new Vector3(-_modelCenterDistance * _modelScale / 2, -_modelCenterDistance * _modelScale / 2, 0), new Vector3(3*MathHelper.PiOver2, 1*MathHelper.PiOver2, 0), new Vector3(_modelScale), gd));
                    // This is south edge
                    _edgeModules.Add(new CustomModel(_edgeModel, Position(i, -edgeDistance) -     new Vector3(0, -_modelCenterDistance * _modelScale / 2, -_modelCenterDistance * _modelScale / 2), new Vector3(3*MathHelper.PiOver2, 0*MathHelper.PiOver2, 0), new Vector3(_modelScale), gd));
                    // This is north edge
                    _edgeModules.Add(new CustomModel(_edgeModel, Position(i, edgeDistance + 1) -  new Vector3(0, -_modelCenterDistance * _modelScale / 2, -_modelCenterDistance * _modelScale / 2), new Vector3(3*MathHelper.PiOver2, 2*MathHelper.PiOver2, 0), new Vector3(_modelScale), gd));
                }
            }
        }
        
        // Gives the position of that grid
        public Vector3 Position(int x, int y)
        {
            return new Vector3(x*_modelCenterDistance, 0, -y*_modelCenterDistance);
        }
        

        // Defines the distance between Origin and the grid's edge
        public int EdgeDistanceFromOrigin(int length)
        {
            return (length - 1) / 2;
        }





        // Draw all the modules of grid and edge
        public void Draw(Matrix View, Matrix Projection, Vector3 CameraPosition)
        {
            foreach (GridModule gridModule in _gridPositions)
            {
                gridModule.Model.Draw(View, Projection, CameraPosition);
            }
            foreach (CustomModel edgeModule in _edgeModules)
            {
                edgeModule.Draw(View, Projection, CameraPosition);
            }
        }



    }

    // A class with two properties, x and y, which also determine the vector3 space the customModel shall be
    public class GridModule
    {
        private Vector3 _position;
        public CustomModel Model;

        // Properties
        private int _x;
        private int _y;

        // Constructor that takes x & y, distance between modules, what scale it has and model to create a customModel
        public GridModule(int x, int y, float originDistance, float scale, Model model, GraphicsDevice graphicsDevice)
        {
            _x = x;
            _y = y;
            
            _position = new Vector3(_x*originDistance, 0, _y*originDistance);
            // Create a new customModel based on given variables
            Model = new CustomModel(model, _position, Vector3.Zero, new Vector3(scale), graphicsDevice);
        }
        
    }
}
