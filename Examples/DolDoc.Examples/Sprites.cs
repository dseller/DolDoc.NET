using DolDoc.Editor.Core;
using DolDoc.Editor.Forms;
using DolDoc.Editor.Sprites;

namespace DolDoc.Examples
{
    [FormHeader("$TI,\"Sprite Demo\"$$FG,RED$Enter the parameters and click the desired sprite type. It will spawn it there.$FG$\n\n")]
    [FormFooter("\n\n")]
    public class SpriteDemoForm
    {
        public SpriteDemoForm()
        {
            Thickness = "1";
        }
        
        [DataField("X1 Coordinate")]
        public string X1 { get; set; }
        
        [DataField("Y1 Coordinate")]
        public string Y1 { get; set; }
        
        [DataField("X2 Coordinate")]
        public string X2 { get; set; }
        
        [DataField("Y2 Coordinate")]
        public string Y2 { get; set; }
        
        [DataField("Thickness")]
        public string Thickness { get; set; }
        
        [DataField("Radius")]
        public string Radius { get; set; }
        
        [ListField("Foreground color", typeof(EgaColor))]
        public EgaColor Color { get; set; }
        
        [DataField("Text (if applicable)")]
        public string Text { get; set; }
        
        [ButtonField("Line", nameof(SpawnLine), prefix: "\n\n   $FG,GREEN$", suffix: "$FG$")]
        public string ButtonLine { get; set; }
        
        [ButtonField("Text box", nameof(SpawnTextBox), prefix: "     ", suffix: null)]
        public string ButtonTextBox { get; set; }
        
        [ButtonField("Text", nameof(SpawnText), prefix: "     ", suffix: null)]
        public string ButtonText { get; set; }
        
        [ButtonField("Circle", nameof(SpawnCircle), prefix: "     ", suffix: null)]
        public string ButtonCircle { get; set; }

        public void SpawnLine(FormDocument<SpriteDemoForm> form)
        {
            var spriteBuilder = new SpriteBuilder(
                new Color(Color),
                new Thick(int.Parse(Thickness)),
                new Line(int.Parse(X1), int.Parse(Y1), int.Parse(X2), int.Parse(Y2))
            );
            var chunk = form.AddChunk(spriteBuilder.Serialize());
            
            form.Write($"$SP,BI={chunk.Id}$\n");
        }
        
        public void SpawnTextBox(FormDocument<SpriteDemoForm> form)
        {
            var spriteBuilder = new SpriteBuilder(
                new Color(Color),
                new Thick(int.Parse(Thickness)),
                new TextBox(int.Parse(X1), int.Parse(Y1), Text)
            );
            var chunk = form.AddChunk(spriteBuilder.Serialize());
            
            form.Write($"$SP,BI={chunk.Id}$\n");
        }
        
        public void SpawnCircle(FormDocument<SpriteDemoForm> form)
        {
            var spriteBuilder = new SpriteBuilder(
                new Color(Color),
                new Thick(int.Parse(Thickness)),
                new Circle(int.Parse(X1), int.Parse(Y1), int.Parse(Radius))
            );
            var chunk = form.AddChunk(spriteBuilder.Serialize());
            
            form.Write($"$SP,BI={chunk.Id}$\n");
        }
        
        public void SpawnText(FormDocument<SpriteDemoForm> form)
        {
            var spriteBuilder = new SpriteBuilder(
                new Color(Color),
                new Thick(int.Parse(Thickness)),
                new Text(int.Parse(X1), int.Parse(Y1), Text)
            );
            var chunk = form.AddChunk(spriteBuilder.Serialize());
            
            form.Write($"$SP,BI={chunk.Id}$\n");
        }
    }
}