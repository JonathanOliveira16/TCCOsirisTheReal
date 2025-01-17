﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OsirisPdvReal.Utils
{
    public class BairroUtil
    {
        public static List<String> GetBairros()
        {
            List<String> Bairros = new List<string> { "Bairro Imperial de São Cristóvão", "Benfica", "Caju", "Catumbi", "Centro", "Cidade Nova", "Estácio", "Gamboa", "Lapa", "Mangueira", "Paquetá", "Rio Comprido", "Santa Teresa", "Santo Cristo", "Saúde", "Vasco da Gama", "Botafogo", "Catete", "Copacabana", "Cosme Velho", "Flamengo", "Gávea", "Glória", "Humaitá", "Ipanema", "Jardim Botânico", "Lagoa", "Laranjeiras", "Leblon", "Leme", "São Conrado", "Urca", "Vidigal", "Rocinha", "Anil", "Barra da Tijuca", "Camorim", "Cidade de Deus", "Curicica", "Freguesia de Jacarepaguá", "Gardênia Azul", "Grumari", "Itanhangá", "Jacarepaguá", "Joá", "Praça Seca", "Pechincha", "Rio das Pedras", "Recreio dos Bandeirantes", "Tanque", "Taquara", "Vargem Grande", "Vargem Pequena", "Vila Valqueire", "Jardim Sulacap", "Bangu", "Campo dos Afonsos", "Deodoro", "Gericinó", "Jabour", "Magalhães Bastos", "Padre Miguel", "Realengo", "Santíssimo", "Senador Camará", "Vila Kennedy", "Vila Militar", "Barra de Guaratiba", "Campo Grande", "Cosmos", "Guaratiba", "Inhoaíba", "Paciência", "Pedra de Guaratiba", "Santa Cruz", "Senador Vasconcelos", "Sepetiba", "Alto da Boa Vista", "Andaraí", "Grajaú", "Maracanã", "Praça da Bandeira", "Tijuca", "Vila Isabel", "Abolição", "Água Santa", "Cachambi", "Del Castilho", "Encantado", "Engenho de Dentro", "Engenho Novo", "Higienópolis", "Jacaré", "Jacarezinho", "Lins de Vasconcelos", "Manguinhos", "Maria da Graça", "Méier", "Piedade", "Pilares", "Riachuelo", "Rocha", "Sampaio", "São Francisco Xavier", "Todos os Santos", "Bonsucesso", "Bancários", "Cacuia", "Cidade Universitária", "Cocotá", "Freguesia", "Galeão", "Jardim Carioca", "Jardim Guanabara", "Maré", "Moneró", "Olaria", "Pitangueiras", "Portuguesa", "Praia da Bandeira", "Ramos", "Ribeira", "Tauá", "Zumbi", "Acari", "Anchieta", "Barros Filho", "Bento Ribeiro", "Brás de Pina", "Campinho", "Cavalcanti", "Cascadura", "Coelho Neto", "Colégio", "Complexo do Alemão", "Cordovil", "Costa Barros", "Engenheiro Leal", "Engenho da Rainha", "Guadalupe", "Honório Gurgel", "Inhaúma", "Irajá", "Jardim América", "Madureira", "Marechal Hermes", "Oswaldo Cruz", "Parada de Lucas", "Parque Anchieta", "Parque Colúmbia", "Pavuna", "Penha", "Penha Circular", "Quintino Bocaiuva", "Ricardo de Albuquerque", "Rocha Miranda", "Tomás Coelho", "Turiaçu", "Vaz Lobo", "Vicente de Carvalho", "Vigário Geral", "Vila da Penha", "Vila Kosmos", "Vista Alegre" };
            return Bairros;
        }

        public static List<String> GetEstados()
        {
            List<String> Estados = new List<String> { "Acre","Alagoas","Amapá","Amazonas","Bahia","Ceará","Espírito Santo","Goiás","Maranhão","Mato Grosso","Mato Grosso do Sul","Minas Gerais","Pará","Paraíba","Paraná","Pernambuco","Piauí","Rio de Janeiro","Rio Grande do Norte","Rio Grande do Sul","Rondônia","Roraima","Santa Catarina","São Paulo","Sergipe","Tocantins" };
            return Estados;
        }
    }
}
