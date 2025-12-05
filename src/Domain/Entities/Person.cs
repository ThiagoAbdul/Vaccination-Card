using Domain.Enums;
using Domain.ValueObjects;
using System.Collections.ObjectModel;

namespace Domain.Entities;

public class Person : AuditableEntity
{
    public Guid Id { get; set; } // Ainda não pretendo utilizar EntityBase com Guid, já me arrependi em uma situação específica.
    public Name Name { get; set; }
    public string CPF { get; set; } // Não pode ser ID, LGPD; a pessoa pode ter informado errado; eu posso deletar (soft delete) e recriar a pessoa.
    public string? RG { get; set; } // Só é único por estado e eu mesmo só fui tirar o meu com 13 anos
    public Gender Gender { get; set; }
    public DateOnly BirthDate { get; set; }
    public List<Vaccination> Vaccinations { get; set; } = []; // Não precisa ser "virtual", não vou usar lazy loading, o time do EF desencoraja o uso.
                                                               // Bom que evita N+1 consultas.
                                                               // Já tive problemas com isso no JPA/Hibernate, que usa lazy loadig por default.

}
