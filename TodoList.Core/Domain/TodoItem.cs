namespace TodoList.Core.Domain
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool IsCompleta { get; set; }
        public TodoItem(string descricao, bool isCompleta = false)
        {
            Descricao = descricao;
            IsCompleta = isCompleta;
        }   
    }
}