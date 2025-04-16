namespace ProjetoEcommerce.Models
{

    public class Produto
    {
        //CRIANDO O ENCAPSULAMENTO DO OBJETO COM GET E SET
        public int CodProd { get; set; } //Acessores
        // Ao usar ?, você está explicitamente dizendo que a propriedade pode intencionalmente ter um valor nulo.
        public string? NomeProd { get; set; }
        public string? DescProd { get; set; }
        public int? QuantProd { get; set; }
        public string? PrecoProd { get; set; }
        public List<Produto>? ListaProduto { get; set; }

    }
}