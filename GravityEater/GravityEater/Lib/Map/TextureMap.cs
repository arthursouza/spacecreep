using System.Collections.Generic;
using System.IO;
using LotusLibrary.Configuration;
using LotusLibrary.DataAccess;

namespace LotusLibrary
{
    public class TextureMap : List<TextureWrapper>
    {
        public static TextureMap LoadTextureMap()
        {
            var dao = new TextureMapDAO();
            TextureMap tm = dao.Select();
            return tm;
        }

        public static void SaveTextureMap(TextureMap map)
        {
            var dao = new TextureMapDAO();
            dao.Insert(map);
            map = LoadTextureMap();
        }

        public static TextureMap CreateFromFolder()
        {
            var txtMap = new TextureMap();
            GeneralConfig.Config = new GeneralConfig();

            var drInfo = new DirectoryInfo(GeneralConfig.Config.TileFolder);
            if (drInfo.Exists)
            {
                foreach (FileInfo file in drInfo.GetFiles())
                {
                    var tw = new TextureWrapper();
                    tw.Name = file.Name.Replace(file.Extension, string.Empty);

                    if (file.Name.Contains("_auto"))
                        tw.TileType = TileType.AutoTile;
                    else
                        tw.TileType = TileType.Common;

                    txtMap.Add(tw);
                }
            }

            SaveTextureMap(txtMap);

            return txtMap;
        }

        public TextureWrapper Find(int id)
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i].TextureId == id)
                    return this[i];
            }
            return null;
        }

        public TextureWrapper Find(string name)
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i].Name == name)
                    return this[i];
            }
            return null;
        }
    }
}