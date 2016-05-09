namespace battleship_zyklop_ki {
    // Helper um Felder um ein Feld herum auszurechnen
    class CoordOffset {
        public static readonly Coord[] aroundTemplate = {
            new Coord(-1, -1), new Coord(-1,  0), new Coord(-1,  1),
            new Coord( 0, -1),                    new Coord( 0,  1),
            new Coord( 1, -1), new Coord( 1,  0), new Coord( 1,  1)
        };

        public static readonly Coord[] plusTemplate = {
                               new Coord(-1, 0),
            new Coord( 0, -1),                   new Coord( 0,  1),
                               new Coord( 1, 0)
        };

        public static readonly Coord[] edgesTemplate = {
            new Coord(-1, -1),                   new Coord(-1,  1),

            new Coord( 1, -1),                   new Coord( 1,  1)
        };

        public static Coord[] around(int x, int y) {
            return around(x, y, 1);
        }

        public static Coord[] around(int x, int y, int by) {
            return offsetWithTemplate(aroundTemplate, x, y, by);
        }

        public static Coord[] plus(int x, int y) {
            return plus(x, y, 1);
        }

        public static Coord[] plus(int x, int y, int by) {
            return offsetWithTemplate(plusTemplate, x, y, by);
        }

        public static Coord[] edges(int x, int y) {
            return edges(x, y, 1);
        }

        public static Coord[] edges(int x, int y, int by) {
            return offsetWithTemplate(edgesTemplate, x, y, by);
        }

        private static Coord[] offsetWithTemplate(Coord[] template, int x, int y, int by) {
            Coord[] coords = new Coord[template.Length];
            for (int i = 0; i < template.Length; i++) {
                coords[i].x = x + template[i].x * by;
                coords[i].y = y + template[i].y * by;
            }
            return coords;
        }
    }
}
