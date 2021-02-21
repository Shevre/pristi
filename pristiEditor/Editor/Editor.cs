using System;
using System.Collections.Generic;
using System.Text;
using pristi.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml;

namespace pristiEditor
{
    public enum EditorState{
        EditingWorld,
        EditingRoom
    }

    public class Editor {
        private EditorState m_State = EditorState.EditingWorld;
        private List<(Room room, Vector2 worldPos)> m_RoomList = new List<(Room room, Vector2 worldPos)>();
        private Texture2D m_WhitePixel;

        Point m_TileSize;

        Point m_BlockSize;
        private float m_Scale = 1.0f;
        private Vector2 m_Offset = new Vector2(0, 0);

        KeyboardState m_LastState;

        private Rectangle m_Bounds;

        private bool m_DrawGrid = true;
        public bool m_Changed { get; private set; } = false;

        private int m_PrevScrollVal = 0;
        private Vector2 m_PrevMousePos = new Vector2();
        private Point m_CurrentMouseTile = new Point();
    

        public Editor(string worldXmlLocation,ContentManager content,GraphicsDevice graphics,Rectangle editorSize,Settings settings){
            m_WhitePixel = new Texture2D(graphics, 1, 1);
            Color[] data = new Color[1] { new Color(255, 255, 255) };
            m_WhitePixel.SetData(data);
            m_Bounds = editorSize;


            LoadXml(worldXmlLocation, content,settings);
            m_LastState = new KeyboardState();
        }

        private void LoadXml(string xmlLocation,ContentManager content,Settings settings){
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlLocation);
            m_TileSize = doc.SelectSingleNode("//World/TileSize").InnerText.ToVector2().ToPoint();
            m_BlockSize = doc.SelectSingleNode("//World/BlockSize").InnerText.ToVector2().ToPoint();
            XmlNodeList roomNodes = doc.SelectNodes("//World/RoomPositions/Room");
            foreach (XmlNode node in roomNodes){
                
                m_RoomList.Add((new Room(settings.BaseRoomFolder + node.SelectSingleNode("XmlLocation").InnerText, content, m_TileSize.X, m_TileSize.Y), node.SelectSingleNode("Position").InnerText.ToVector2()));
                Console.WriteLine(m_RoomList[m_RoomList.Count-1]);
            }
        }

        public void Delete(){
            m_WhitePixel.Dispose();
        }


        public void Update(){

            if (Keyboard.GetState().IsKeyDown(Keys.G) && m_LastState.IsKeyUp(Keys.G))
                m_DrawGrid = !m_DrawGrid;

            m_Scale += (Mouse.GetState().ScrollWheelValue - m_PrevScrollVal == 0) ? 0f : (Mouse.GetState().ScrollWheelValue - m_PrevScrollVal < 0) ? -0.25f : 0.25f;
            m_Scale = (m_Scale < 0.25f) ? 0.25f : m_Scale;
            if(Mouse.GetState().RightButton == ButtonState.Pressed){
                m_Offset -= m_PrevMousePos - Mouse.GetState().Position.ToVector2();
            }
            m_CurrentMouseTile = ((Mouse.GetState().Position.ToVector2()-m_Offset - new Vector2(m_Bounds.X,m_Bounds.Y)) / (m_TileSize.ToVector2() * m_Scale)).ToPoint();

            m_LastState = Keyboard.GetState();
            m_PrevScrollVal = Mouse.GetState().ScrollWheelValue;
            m_PrevMousePos = Mouse.GetState().Position.ToVector2();
        }

        

        public void Draw(SpriteBatch spriteBatch){
            if (m_DrawGrid){//Is bad and sucks but it works so is fine
                for (int y = 0; y < (m_Bounds.Height/(m_TileSize.Y*m_Scale))+2; y++){
                    
                    for (int x = (y%2==0)? 0:1; x < (m_Bounds.Width / (m_TileSize.X*m_Scale))+(2*m_Scale); x+=2){
                        if(m_Offset.X <= 0 && m_Offset.Y <= 0)
                            spriteBatch.FillRectangle(m_WhitePixel, new Rectangle(((int)m_Offset.X) % (int)((m_TileSize.X * m_Scale) * 2) + (int)(x * m_TileSize.X * m_Scale), ((int)m_Offset.Y) % (int)((m_TileSize.Y * m_Scale) * 2) + (int)(y * m_TileSize.Y * m_Scale), (int)(m_TileSize.X * m_Scale), (int)(m_TileSize.Y * m_Scale)), Color.Gray);
                        else if(m_Offset.X <= 0)
                            spriteBatch.FillRectangle(m_WhitePixel, new Rectangle(((int)m_Offset.X) % (int)((m_TileSize.X * m_Scale) * 2) + (int)(x * m_TileSize.X * m_Scale), ((int)m_Offset.Y)/* % (int)((m_TileSize.Y * m_Scale) * 2)*/ + (int)(y * m_TileSize.Y * m_Scale), (int)(m_TileSize.X * m_Scale), (int)(m_TileSize.Y * m_Scale)), Color.Gray);
                        else if (m_Offset.Y <= 0)
                            spriteBatch.FillRectangle(m_WhitePixel, new Rectangle(((int)m_Offset.X)/* % (int)((m_TileSize.X * m_Scale) * 2)*/ + (int)(x * m_TileSize.X * m_Scale), ((int)m_Offset.Y) % (int)((m_TileSize.Y * m_Scale) * 2) + (int)(y * m_TileSize.Y * m_Scale), (int)(m_TileSize.X * m_Scale), (int)(m_TileSize.Y * m_Scale)), Color.Gray);
                        else
                            spriteBatch.FillRectangle(m_WhitePixel, new Rectangle(((int)m_Offset.X)/* % (int)((m_TileSize.X * m_Scale) * 2)*/ + (int)(x * m_TileSize.X * m_Scale), ((int)m_Offset.Y) /*% (int)((m_TileSize.Y * m_Scale) * 2)*/ + (int)(y * m_TileSize.Y * m_Scale), (int)(m_TileSize.X * m_Scale), (int)(m_TileSize.Y * m_Scale)), Color.Gray);
                    }
                }
            }

            foreach ((Room room,Vector2 pos) r in m_RoomList){
                r.room.Draw(spriteBatch,m_Offset + r.pos*m_TileSize.ToVector2()*m_Scale,m_Scale);
                spriteBatch.DrawRectangle(m_WhitePixel, new Rectangle((int)m_Offset.X + (int)(r.pos.X * m_TileSize.X*m_Scale), (int)m_Offset.Y + (int)(r.pos.Y * m_TileSize.Y*m_Scale), (int)(r.room.GetWidth() * m_TileSize.X*m_Scale), (int)(r.room.GetHeight() * m_TileSize.Y*m_Scale)), Color.Black, 1);
            }
            spriteBatch.FillRectangle(m_WhitePixel, new Rectangle((int)m_Offset.X+(int)(m_CurrentMouseTile.X * m_TileSize.X * m_Scale), (int)m_Offset.Y+(int)(m_CurrentMouseTile.Y * m_TileSize.Y * m_Scale), (int)(m_TileSize.X * m_Scale), (int)(m_TileSize.Y * m_Scale)),Color.DeepPink);

            //switch (state)
            //{
            //    case EditorState.EditingWorld:
            //        break;
            //    case EditorState.EditingRoom:
            //        break;
            //    default:
            //        break;
            //}
        }

        public void DrawGui(){
            
        }
    }
}
