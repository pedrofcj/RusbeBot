using Discord;
using Discord.Interactions;

namespace RusbeBot.Core.Components;

public class SugestaoModal : IModal
{
    public string Title => "Sugestão";

    [InputLabel("Descrição")]
    [ModalTextInput("sugestao_description", TextInputStyle.Paragraph, "Descreva sua sugestão aqui")]
    public string? Description { get; set; }
}